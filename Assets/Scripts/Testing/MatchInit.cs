using UnityEngine;

public class MatchInit : MonoBehaviour
{
    [Header("Fallback (used only if no player data was passed in)")]
    public GameObject boatPrefab;
    public int fallbackPlayerCount = 2;

    private void Start()
    {
        if (LobbyManager.GetPlayerData().Length == 0)
        {
            LobbyManager.SetupTestPlayers(boatPrefab, fallbackPlayerCount);
        }

        FindAnyObjectByType<PlayerBoatSpawner>().SpawnAllPlayers();
    }
}