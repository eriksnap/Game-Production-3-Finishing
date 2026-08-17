using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    [Header("Player Slots")]
    public GameObject[] playerSlots;
    public TextMeshProUGUI[] playerStatusTexts;
    public Image[] playerReadyIndicators;

    [Header("Countdown")]
    public TextMeshProUGUI countdownText;
    public GameObject countdownPanel;

    [Header("Colors")]
    public Color readyColor = Color.green;
    public Color notReadyColor = Color.red;
    public Color emptySlotColor = Color.grey;

    [Header("Join Prompt")]
    public TextMeshProUGUI joinPromptText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitialiseSlots();
        countdownPanel?.SetActive(false);
        joinPromptText?.gameObject.SetActive(true);
    }

    private void InitialiseSlots()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (playerStatusTexts[i] != null)
                playerStatusTexts[i].text = "Waiting...";

            if (playerReadyIndicators[i] != null)
                playerReadyIndicators[i].color = emptySlotColor;
        }
    }

    public void OnPlayerJoined(int playerIndex)
    {
        if (playerIndex >= playerSlots.Length) return;

        if (playerStatusTexts[playerIndex] != null)
            playerStatusTexts[playerIndex].text = $"Player {playerIndex + 1}";

        if (playerReadyIndicators[playerIndex] != null)
            playerReadyIndicators[playerIndex].color = notReadyColor;

        // Hide join prompt once at least one player joins
        joinPromptText?.gameObject.SetActive(false);
    }

    public void UpdateReadyStatus(int playerIndex, bool isReady)
    {
        if (playerIndex >= playerSlots.Length) return;

        if (playerStatusTexts[playerIndex] != null)
            playerStatusTexts[playerIndex].text = isReady
                ? $"Player {playerIndex + 1} - Ready!"
                : $"Player {playerIndex + 1}";

        if (playerReadyIndicators[playerIndex] != null)
            playerReadyIndicators[playerIndex].color = isReady ? readyColor : notReadyColor;
    }

    public void UpdateCountdown(int value, bool visible)
    {
        if (countdownPanel == null) return;

        countdownPanel.SetActive(visible);

        if (countdownText != null && visible)
            countdownText.text = $"Starting in {value}...";
    }
}