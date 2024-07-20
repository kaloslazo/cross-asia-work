using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;  // Asegúrate de incluir este espacio de nombres

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public Transform playerHead;
    public float spawnDistance = 1.5f;
    public InputActionReference openMenuAction;

    [Header("Menu Buttons")]
    public Button resumeButton;
    public Button quitButton;

    private void Awake()
    {
        openMenuAction.action.Enable();
        openMenuAction.action.performed += ToggleMenu;
        InputSystem.onDeviceChange += OnDeviceChange;

        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OnDestroy()
    {
        openMenuAction.action.Disable();
        openMenuAction.action.performed -= ToggleMenu;
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        bool isMenuActive = !menuPanel.activeSelf;
        menuPanel.SetActive(isMenuActive);
        Time.timeScale = isMenuActive ? 0 : 1;

        if (isMenuActive)
        {
            // Calcula la posición del menú en frente del jugador
            Vector3 forwardFlat = new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized;
            menuPanel.transform.position = playerHead.position + forwardFlat * spawnDistance;
            menuPanel.transform.rotation = Quaternion.LookRotation(forwardFlat);
        }
    }

    private void ResumeGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void QuitGame()
    {
        Time.timeScale = 1;
        var loadOperation = SceneManager.LoadSceneAsync("MenuScene");
        loadOperation.completed += (AsyncOperation operation) =>
        {
            Debug.Log("Scene Loaded");
        };
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Disconnected)
        {
            openMenuAction.action.Disable();
        }
        else if (change == InputDeviceChange.Reconnected)
        {
            openMenuAction.action.Enable();
        }
    }
}
