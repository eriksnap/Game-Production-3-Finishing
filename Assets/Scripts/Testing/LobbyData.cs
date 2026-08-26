using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class LobbyData
{
    public int playerIndex;
    public GameObject selectedBoatPrefab;
    public GameObject selectedCharacterPrefab;
    [System.NonSerialized] public InputDevice assignedDevice;
}