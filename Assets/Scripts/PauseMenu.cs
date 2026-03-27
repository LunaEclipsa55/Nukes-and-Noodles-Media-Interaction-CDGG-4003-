using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private bool isPaused;

    void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");

    }
        
    void Start()
    {
        pauseMenu.SetActive(false);
            
            
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            GameObject quitButton;
            quitButton = GameObject.Find("QuitButton");
            quitButton.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        if (Application.isPlaying)
            Application.Quit();
        else Debug.Log("Quitting Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Capturing");
    }
}

