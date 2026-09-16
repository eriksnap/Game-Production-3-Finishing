using UnityEngine;
using UnityEngine.EventSystems;   

public class ControlsPanelToggle : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject mainMenuButtonsPanel;

    [Header("UI Navigation")]
    public GameObject firstButtonInControlsPanel;
    public GameObject firstButtonInMainPanel; 

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
        mainMenuButtonsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstButtonInControlsPanel);  
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
        mainMenuButtonsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstButtonInMainPanel);
    }
}