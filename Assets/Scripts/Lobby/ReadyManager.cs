using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ReadyManager : MonoBehaviour
{
    public static ReadyManager Instance { get; private set; }

    [Header("Settings")]
    public float countdownDuration = 5f;
    public string gameSceneName = "RC-BoatScene";

    private Dictionary<LobbyPlayerController, bool> readyStates = new Dictionary<LobbyPlayerController, bool>();
    private Coroutine countdownCoroutine;
    private bool isCountingDown = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ToggleReady(LobbyPlayerController player)
    {
        if (!readyStates.ContainsKey(player))
            readyStates[player] = false;

        readyStates[player] = !readyStates[player];

        LobbyUI.Instance?.UpdateReadyStatus(player.PlayerIndex, readyStates[player]);

        CheckAllReady();
    }

    private void CheckAllReady()
    {
        int joinedCount = LobbyPlayerJoinManager.Instance.GetJoinedPlayerCount();

        if (joinedCount < 2)
        {
            StopCountdown();
            return;
        }

        bool allReady = true;
        foreach (var state in readyStates.Values)
        {
            if (!state)
            {
                allReady = false;
                break;
            }
        }

        // Also check that all joined players have a ready state registered
        if (readyStates.Count < joinedCount)
            allReady = false;

        if (allReady)
            StartCountdown();
        else
            StopCountdown();
    }

    private void StartCountdown()
    {
        if (isCountingDown) return;
        isCountingDown = true;
        countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    private void StopCountdown()
    {
        if (!isCountingDown) return;
        isCountingDown = false;

        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        LobbyUI.Instance?.UpdateCountdown(0, false);
    }

    private IEnumerator CountdownRoutine()
    {
        float timer = countdownDuration;

        while (timer > 0f)
        {
            LobbyUI.Instance?.UpdateCountdown(Mathf.CeilToInt(timer), true);
            yield return new WaitForSeconds(1f);
            timer--;

            // Re-check in case someone unreadied
            if (!isCountingDown) yield break;
        }

        LoadGame();
    }

    private void LoadGame()
    {
        LobbyPlayerJoinManager.Instance.BuildLobbyData();
        SceneManager.LoadScene(gameSceneName);
    }
}