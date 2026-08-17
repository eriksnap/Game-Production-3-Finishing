using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class LobbyPlayerJoinManager : MonoBehaviour
{
    public static LobbyPlayerJoinManager Instance { get; private set; }

    [Header("Player Setup")]
    public GameObject lobbyCharacterPrefab;
    public Transform[] spawnPoints;
    public GameObject playerInputPrefab;

    [Header("Default Selections")]
    public GameObject defaultBoatPrefab;
    public GameObject defaultCharacterPrefab;

    private List<LobbyPlayerController> joinedPlayers = new List<LobbyPlayerController>();
    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = joinedPlayers.Count;

        if (index >= spawnPoints.Length)
        {
            Debug.LogWarning("No spawn point available for this player.");
            return;
        }

        // Get the controller component
        LobbyPlayerController controller = playerInput.GetComponent<LobbyPlayerController>();
        if (controller == null)
        {
            Debug.LogWarning("LobbyPlayerController not found on joined player.");
            return;
        }

        // Set defaults
        controller.PlayerIndex = index;
        controller.SelectedBoatPrefab = defaultBoatPrefab;
        controller.SelectedCharacterPrefab = defaultCharacterPrefab;

        // Move to spawn point
        playerInput.transform.position = spawnPoints[index].position;
        playerInput.transform.rotation = spawnPoints[index].rotation;

        joinedPlayers.Add(controller);

        // Notify UI
        LobbyUI.Instance?.OnPlayerJoined(index);

        // Notify camera manager to assign camera
        CameraManager.Instance?.AssignCamera(playerInput.gameObject, index);

        Debug.Log($"Player {index + 1} joined the lobby.");
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
                assignedDevice = joinedPlayers[i].GetComponent<PlayerInput>().devices[0]
            };
            dataList.Add(data);
        }

        LobbyManager.SetPlayerData(dataList);
    }
}