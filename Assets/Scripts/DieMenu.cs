using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DieMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public Button quitButton;
    public Transform playerHead;
    public float spawnDistance = 1.5f;

    private void Awake()
    {
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ShowDieMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0;

        if (!playerHead)
            playerHead = Camera.main.transform;

        Vector3 forwardFlat = new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized;
        menuPanel.transform.position = playerHead.position + forwardFlat * spawnDistance;
        menuPanel.transform.rotation = Quaternion.LookRotation(forwardFlat);
    }

    private void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}
