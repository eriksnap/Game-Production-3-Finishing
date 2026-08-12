using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        Countdown,
        Active,
        GameOver
    }

    [Header("Settings")]
    public float countdownDuration = 3f;
    public float gameOverDisplayDuration = 5f;

    [Header("Scene Names")]
    public string lobbySceneName = "LobbyScene";

    [Header("UI")]
    public GameHUD gameHUD;

    private GameState currentState;
    private List<GameObject> activePlayers = new List<GameObject>();
    private List<int> eliminatedPlayers = new List<int>();
    private int winnerIndex = -1;

    public void InitialiseGame(List<GameObject> spawnedBoats)
    {
        activePlayers = new List<GameObject>(spawnedBoats);
        eliminatedPlayers.Clear();
        winnerIndex = -1;

        // Disable all boat controllers until countdown finishes
        SetAllBoatsActive(false);

        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        currentState = GameState.Countdown;
        float timer = countdownDuration;

        while (timer > 0f)
        {
            gameHUD?.UpdateCountdown(Mathf.CeilToInt(timer));
            yield return new WaitForSeconds(1f);
            timer--;
        }

        gameHUD?.HideCountdown();
        StartGame();
    }

    private void StartGame()
    {
        currentState = GameState.Active;
        SetAllBoatsActive(true);
    }

    public void OnPlayerEliminated(int playerIndex)
    {
        if (currentState != GameState.Active) return;
        if (eliminatedPlayers.Contains(playerIndex)) return;

        eliminatedPlayers.Add(playerIndex);
        gameHUD?.ShowEliminatedMessage(playerIndex);

        // Disable the eliminated boat's controller
        GameObject eliminatedBoat = activePlayers[playerIndex];
        eliminatedBoat?.GetComponent<BoatController>()?.SetEliminated();

        CheckForWinner();
    }

    private void CheckForWinner()
    {
        List<int> remaining = new List<int>();

        for (int i = 0; i < activePlayers.Count; i++)
        {
            if (!eliminatedPlayers.Contains(i))
            {
                remaining.Add(i);
            }
        }

        if (remaining.Count == 1)
        {
            winnerIndex = remaining[0];
            StartCoroutine(GameOverRoutine());
        }
        else if (remaining.Count == 0)
        {
            // All eliminated simultaneously — draw
            winnerIndex = -1;
            StartCoroutine(GameOverRoutine());
        }
    }

    private IEnumerator GameOverRoutine()
    {
        currentState = GameState.GameOver;
        SetAllBoatsActive(false);

        if (winnerIndex >= 0)
            gameHUD?.ShowWinner(winnerIndex);
        else
            gameHUD?.ShowDraw();

        yield return new WaitForSeconds(gameOverDisplayDuration);

        ReturnToLobby();
    }

    private void SetAllBoatsActive(bool active)
    {
        foreach (GameObject boat in activePlayers)
        {
            if (boat == null) continue;

            BoatController controller = boat.GetComponent<BoatController>();
            if (controller != null) controller.enabled = active;

            PlayerInput input = boat.GetComponent<PlayerInput>();
            if (input != null) input.enabled = active;
        }
    }

    private void ReturnToLobby()
    {
        SceneManager.LoadScene(lobbySceneName);
    }

    public GameState GetCurrentState() => currentState;
    public int GetWinnerIndex() => winnerIndex;
}