using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class LobbyPlayerJoinManager : MonoBehaviour
{
    public static LobbyPlayerJoinManager Instance { get; private set; }

    [Header("Player Setup")]
    public GameObject lobbyCharacterPrefab;
    public Transform[] spawnPoints;

    [Header("Default Selections")]
    public GameObject defaultBoatPrefab;
    public GameObject defaultCharacterPrefab;

    private List<LobbyPlayerController> joinedPlayers = new List<LobbyPlayerController>();
    private List<InputDevice> joinedDevices = new List<InputDevice>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && !joinedDevices.Contains(gamepad))
            {
                JoinPlayer(gamepad);
            }
        }
    }

    private void JoinPlayer(InputDevice device)
    {
        int index = joinedPlayers.Count;

        if (index >= spawnPoints.Length)
        {
            Debug.LogWarning("No spawn point available.");
            return;
        }

        if (index >= 4)
        {
            Debug.LogWarning("Max players reached.");
            return;
        }

        // Spawn character
        GameObject playerObject = Instantiate(lobbyCharacterPrefab,
            spawnPoints[index].position,
            spawnPoints[index].rotation);

        playerObject.name = $"Player{index + 1}_Character";

        // Assign PlayerInput device
        PlayerInput playerInput = playerObject.GetComponent<PlayerInput>();
        if (playerInput != null)
            playerInput.SwitchCurrentControlScheme(device);

        // Set up controller
        LobbyPlayerController controller = playerObject.GetComponent<LobbyPlayerController>();
        if (controller != null)
        {
            controller.PlayerIndex = index;
            controller.SelectedBoatPrefab = defaultBoatPrefab;
            controller.SelectedCharacterPrefab = defaultCharacterPrefab;
        }
        else
        {
            Debug.LogWarning($"LobbyPlayerController not found on {playerObject.name}");
        }

        joinedDevices.Add(device);
        joinedPlayers.Add(controller);

        // Notify UI and camera
        LobbyUI.Instance?.OnPlayerJoined(index);
        CameraManager.Instance?.AssignCamera(playerObject, index);

        Debug.Log($"Player {index + 1} joined using {device.displayName}");
    }

    public List<LobbyPlayerController> GetJoinedPlayers() => joinedPlayers;
    public int GetJoinedPlayerCount() => joinedPlayers.Count;

    public void BuildLobbyData()
    {
        List<LobbyData> dataList = new List<LobbyData>();
        for (int i = 0; i < joinedPlayers.Count; i++)
        {
            LobbyData data = new LobbyData
            {
                playerIndex = i,
                selectedBoatPrefab = joinedPlayers[i].SelectedBoatPrefab,
                selectedCharacterPrefab = joinedPlayers[i].SelectedCharacterPrefab,
                assignedDevice = joinedDevices[i]
            };
            dataList.Add(data);
        }
        LobbyManager.SetPlayerData(dataList);
    }
}