using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField] private GameObject pauseMenuPanel;

    private void Start()
    {
        Resume();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        GameManager.Instance.isReceiveInput = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        GameManager.Instance.isReceiveInput = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

}
