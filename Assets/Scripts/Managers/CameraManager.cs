using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Header("Cinemachine")]
    public CinemachineCamera cinemachineCameraPrefab;

    [Header("Split Screen Rects")]
    //These define the screen area each camera renders to
    private readonly Rect[] splitScreenRects = new Rect[]
    {
        new Rect(0f, 0.5f, 0.5f, 0.5f),   // Player 1
        new Rect(0.5f, 0.5f, 0.5f, 0.5f), // Player 2
        new Rect(0f, 0f, 0.5f, 0.5f),     // Player 3
        new Rect(0.5f, 0f, 0.5f, 0.5f)    // Player 4
    };

    private List<Camera> playerCameras = new List<Camera>();
    private List<CinemachineCamera> cinemachineCameras = new List<CinemachineCamera>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AssignCamera(GameObject playerObject, int playerIndex)
    {
        //Create a new Unity Camera
        GameObject cameraObject = new GameObject($"Player{playerIndex + 1}_Camera");
        Camera cam = cameraObject.AddComponent<Camera>();
        cam.rect = GetSplitRect(playerIndex);
        cam.depth = playerIndex;

        //Create Cinemachine camera
        CinemachineCamera cmCam = Instantiate(cinemachineCameraPrefab);
        cmCam.name = $"Player{playerIndex + 1}_CinemachineCamera";

        //Set follow and look at targets
        cmCam.Follow = playerObject.transform;
        cmCam.LookAt = playerObject.transform;

        //Link Cinemachine camera to this player's Unity camera
        var brain = cameraObject.AddComponent<CinemachineBrain>();
        brain.ChannelMask = (OutputChannels)(1 << playerIndex);
        cmCam.OutputChannel = (OutputChannels)(1 << playerIndex);

        //Set third person offset
        var composer = cmCam.GetComponent<CinemachineFollow>();
        if (composer != null)
        {
            composer.FollowOffset = new Vector3(0f, 5f, -7f);
        }

        playerCameras.Add(cam);
        
        PlayerInput playerInput = playerObject.GetComponent<PlayerInput>();
        if (playerInput != null)
            playerInput.camera = cam;
        
        cinemachineCameras.Add(cmCam);

        UpdateSplitScreen();

        //Assign camera transform to player controller
        LobbyPlayerController controller = playerObject.GetComponent<LobbyPlayerController>();
        if (controller != null)
            controller.cameraTransform = cameraObject.transform;
    }

    private void UpdateSplitScreen()
    {
        int count = playerCameras.Count;

        for (int i = 0; i < count; i++)
        {
            playerCameras[i].rect = GetSplitRect(i, count);
        }
    }

    private Rect GetSplitRect(int index, int totalPlayers = 1)
    {
        if (totalPlayers == 1)
            return new Rect(0f, 0f, 1f, 1f); // Fullscreen for single player

        if (totalPlayers == 2)
        {
            return index == 0
                ? new Rect(0f, 0f, 0.5f, 1f)   //Left half
                : new Rect(0.5f, 0f, 0.5f, 1f); //Right half
        }

        // 3 or 4 players - quad split
        return splitScreenRects[index];
    }
}