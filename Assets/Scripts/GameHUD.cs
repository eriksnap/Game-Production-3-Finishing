using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Countdown")]
    public TextMeshProUGUI countdownText;

    [Header("Game Over")]
    public TextMeshProUGUI gameOverText;

    [Header("Eliminated")]
    public TextMeshProUGUI eliminatedText;

    private void Start()
    {
        HideCountdown();
        HideGameOver();
        HideEliminated();
    }

    public void UpdateCountdown(int value)
    {
        if (countdownText == null) return;
        countdownText.gameObject.SetActive(true);
        countdownText.text = value > 0 ? value.ToString() : "GO!";
    }

    public void HideCountdown()
    {
        if (countdownText == null) return;
        countdownText.gameObject.SetActive(false);
    }

    public void ShowEliminatedMessage(int playerIndex)
    {
        if (eliminatedText == null) return;
        eliminatedText.gameObject.SetActive(true);
        eliminatedText.text = $"Player {playerIndex + 1} Eliminated!";
        Invoke(nameof(HideEliminated), 2f);
    }

    public void HideEliminated()
    {
        if (eliminatedText == null) return;
        eliminatedText.gameObject.SetActive(false);
    }

    public void ShowWinner(int playerIndex)
    {
        if (gameOverText == null) return;
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = $"Player {playerIndex + 1} Wins!";
    }

    public void ShowDraw()
    {
        if (gameOverText == null) return;
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Draw!";
    }

    public void HideGameOver()
    {
        if (gameOverText == null) return;
        gameOverText.gameObject.SetActive(false);
    }
}