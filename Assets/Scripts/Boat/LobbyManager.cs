using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    private static List<LobbyData> playerDataList = new List<LobbyData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void SetPlayerData(List<LobbyData> data)
    {
        playerDataList = data;
    }

    public static LobbyData[] GetPlayerData()
    {
        return playerDataList.ToArray();
    }

    public static void ClearPlayerData()
    {
        playerDataList.Clear();
    }

    // Temporary: for testing in game scene without going through lobby
    public static void SetupTestPlayers(GameObject boatPrefab, int playerCount)
    {
        playerDataList.Clear();

        for (int i = 0; i < playerCount; i++)
        {
            LobbyData data = new LobbyData
            {
                playerIndex = i,
                selectedBoatPrefab = boatPrefab,
                selectedCharacterPrefab = null,
                assignedDevice = i == 0 && Gamepad.all.Count > 0
                    ? Gamepad.all[0]
                    : null // Player 2 gets null, falls back to keyboard
            };

            playerDataList.Add(data);
        }
    }
}