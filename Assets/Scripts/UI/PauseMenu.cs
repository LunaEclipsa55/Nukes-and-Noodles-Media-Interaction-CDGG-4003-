using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference pauseAction;
    
    [Header("UI")]
    [SerializeField] GameObject pauseMenu;

    private bool isPaused = false;

    void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");

    }
        
    void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(isPaused);
    }
    
    
    private void OnEnable()
    {
        // Subscribe to the action
        pauseAction.action.performed += OnPausePerformed;
        pauseAction.action.Enable();
    }
    
    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        pauseAction.action.performed -= OnPausePerformed;
        pauseAction.action.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f; // Freeze/unfreeze game time
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(isPaused);
        
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        
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
        //PlayerStats.isDead = false;
        //Destroy(GameObject.FindGameObjectWithTag("Player"));
        Time.timeScale = 1f;
        PlayerStats.isDead = false;

        
        string scene = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        
        ScoreManager.Instance.ResetScore();

        PlayerMovement.isFacingRight = true; 
    }

    public void RestartGameDied()
    {
        Time.timeScale = 1f;

        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }
}

