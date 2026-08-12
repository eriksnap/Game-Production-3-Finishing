using UnityEngine;

public class TestInit : MonoBehaviour
{
    public GameObject boatPrefab;
    public int playerCount = 2;

    private void Start()
    {
        LobbyManager.SetupTestPlayers(boatPrefab, playerCount);
        FindAnyObjectByType<PlayerBoatSpawner>().SpawnAllPlayers();
    }
}