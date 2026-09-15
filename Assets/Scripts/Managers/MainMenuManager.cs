using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "RC-BoatScene";

    [Header("Boat Prefab (temporary until Lobby is finished)")]
    public GameObject boatPrefab;

    [Header("UI")]
    public GameObject firstSelectedButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void OnPlayPressed()
    {
        int gamepadCount = Gamepad.all.Count;
        int playerCount = Mathf.Clamp(gamepadCount, 2, 4); // enforce at least 2, cap at 4

        LobbyManager.ClearPlayerData();
        LobbyManager.SetupTestPlayers(boatPrefab, playerCount);

        SceneManager.LoadScene(gameSceneName);
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}