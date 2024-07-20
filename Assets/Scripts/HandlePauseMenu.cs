using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private XRIDefaultInputActions inputActions;

    private void Awake()
    {
        inputActions = new XRIDefaultInputActions();

        inputActions.PauseMenu.Pause.performed += _ => TogglePause();
        inputActions.PauseMenu.Resume.performed += _ => Resume();
        inputActions.PauseMenu.Exit.performed += _ => QuitGame();

        inputActions.PauseMenu.Pause.Enable();
        inputActions.PauseMenu.Resume.Enable();
        inputActions.PauseMenu.Exit.Enable();
    }

    private void OnDestroy()
    {
        inputActions.PauseMenu.Pause.Disable();
        inputActions.PauseMenu.Resume.Disable();
        inputActions.PauseMenu.Exit.Disable();
    } 

    private void TogglePause()
    {
        Debug.Log("Toggle pause called");
        bool isActive = pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(!isActive);
        Time.timeScale = isActive ? 1 : 0;
        Debug.Log("Menu is now " + (isActive ? "inactive" : "active"));
    }

    private void Resume()
    {
        Debug.Log("Resume called");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }

    private void QuitGame()
    {
        Debug.Log("Quit game called");
        Application.Quit();
    }

}
