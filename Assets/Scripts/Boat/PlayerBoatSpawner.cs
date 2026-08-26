using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerBoatSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Fallback Prefab")]
    public GameObject defaultBoatPrefab;

    private GameStateManager gameStateManager;
    private List<GameObject> spawnedBoats = new List<GameObject>();

    private void Awake()
    {
        gameStateManager = GetComponent<GameStateManager>();
    }

    public void SpawnAllPlayers()
    {
        if (gameStateManager == null)
        {
            gameStateManager = GetComponent <GameStateManager>();
        }

        LobbyData[] players = LobbyManager.GetPlayerData();

        for (int i = 0; i < players.Length; i++)
        {
            if (i >= spawnPoints.Length)
            {
                Debug.LogWarning($"Not enough spawn points for player {i + 1}");
                break;
            }

            GameObject prefabToSpawn = players[i].selectedBoatPrefab != null
                ? players[i].selectedBoatPrefab
                : defaultBoatPrefab;

            //Spawn the boat
            GameObject boat = Instantiate(prefabToSpawn, spawnPoints[i].position, spawnPoints[i].rotation);
            boat.name = $"Player{i + 1}_Boat";

            //Apply the player's colour to boat
            Renderer[] renderers = boat.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                foreach (Material mat in r.materials)
                {
                    mat.color = PlayerColours.Get(i);
                }
            }

            //Assigns the correct gamepad via PlayerInput
            PlayerInput playerInput = boat.GetComponent<PlayerInput>();
            if (playerInput != null && players[i].assignedDevice != null)
            {
                playerInput.SwitchCurrentControlScheme(players[i].assignedDevice);
            }

            BoatHealth health = boat.GetComponent<BoatHealth>();
            if (health != null)
            {
                health.SetPlayerIndex(i);
                int playerIndex = i;
                health.onEliminated.AddListener(() =>
                    gameStateManager.OnPlayerEliminated(playerIndex));
            }

            //Swap character model if selected
            BoatCharacterSlot characterSlot = boat.GetComponentInChildren<BoatCharacterSlot>();
            if (characterSlot != null && players[i].selectedCharacterPrefab != null)
            {
                characterSlot.SetCharacter(players[i].selectedCharacterPrefab);
            }

            spawnedBoats.Add(boat);
        }

        gameStateManager.InitialiseGame(spawnedBoats);
    }

    public List<GameObject> GetSpawnedBoats() => spawnedBoats;
}