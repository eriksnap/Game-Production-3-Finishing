using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Player HUDs")]
    public Slider[] hpBars;
    public TextMeshProUGUI[] playerLabels;

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

    public void InitialiseHUD(int playerCount)
    {
        for (int i = 0; i < hpBars.Length; i++)
        {
            bool active = i < playerCount;

            if (hpBars[i] != null)
                hpBars[i].gameObject.SetActive(active);

            if (playerLabels[i] != null)
            {
                playerLabels[i].gameObject.SetActive(active);
                playerLabels[i].text = $"Player {i + 1}";
                playerLabels[i].color = PlayerColours.Get(i);
            }

            //Colour the HP bar fill to match player colour
            if (hpBars[i] != null)
            {
                Image fill = hpBars[i].fillRect.GetComponent<Image>();
                if (fill != null)
                    fill.color = PlayerColours.Get(i);
            }
        }
    }

    public void UpdateHP(int playerIndex, float currentHP, float maxHP)
    {
        if (playerIndex >= hpBars.Length) return;
        if (hpBars[playerIndex] == null) return;

        float fraction = currentHP / maxHP;
        hpBars[playerIndex].value = currentHP;
        hpBars[playerIndex].maxValue = maxHP;

        //Change colour green to red based on HP
        Image fill = hpBars[playerIndex].fillRect.GetComponent<Image>();
        if (fill != null)
            fill.color = Color.Lerp(Color.red, Color.green, fraction);
    }

    public void ShowEliminatedOnHUD(int playerIndex)
    {
        if (playerIndex >= playerLabels.Length) return;
        if (playerLabels[playerIndex] != null)
            playerLabels[playerIndex].text = $"Player {playerIndex + 1} DEAD";

        if (hpBars[playerIndex] != null)
        {
            Image fill = hpBars[playerIndex].fillRect.GetComponent<Image>();
            if (fill != null) fill.color = Color.grey;
        }
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
        gameOverText.color = PlayerColours.Get(playerIndex);
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