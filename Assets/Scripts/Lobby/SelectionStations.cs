using UnityEngine;
using TMPro;

public class SelectionStation : MonoBehaviour
{
    public enum StationType { Character, Boat }

    [Header("Station Type")]
    public StationType stationType;

    [Header("Options")]
    public GameObject[] options;
    public string[] optionNames;

    [Header("UI")]
    public TextMeshProUGUI stationNameText;
    public TextMeshProUGUI currentSelectionText;

    private int currentIndex = 0;

    private void Start()
    {
        UpdateUI();
    }

    public void Interact(LobbyPlayerController player)
    {
        //Cycle to next option
        currentIndex = (currentIndex + 1) % options.Length;

        //Assign selection to player
        if (stationType == StationType.Character)
            player.SelectedCharacterPrefab = options[currentIndex];
        else
            player.SelectedBoatPrefab = options[currentIndex];

        UpdateUI();

        Debug.Log($"Player {player.PlayerIndex + 1} selected {optionNames[currentIndex]}");
    }

    private void UpdateUI()
    {
        if (currentSelectionText != null && optionNames.Length > currentIndex)
            currentSelectionText.text = optionNames[currentIndex];
    }
}