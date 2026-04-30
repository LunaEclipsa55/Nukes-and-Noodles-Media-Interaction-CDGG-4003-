using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    private bool isInsideTrigger = false;
    private bool isInsideNextStageTrigger = false;

    [Header("Input")] public InputActionReference interact;

    [Header("UI")] public GameObject wonUI;
    public Text scoreText;

    public GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "WonDoor")
        {
            isInsideTrigger = true;
        }

        if (other.CompareTag("NextLevel"))
        {
            Debug.Log("Nextstage enter");

            isInsideNextStageTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "WonDoor")
        {
            isInsideTrigger = false;
        }

        if (other.CompareTag("NextLevel"))
        {
            Debug.Log("Nextstage exit");
            isInsideNextStageTrigger = false;
        }
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.action.name + " clicked");
        if (isInsideTrigger && ctx.performed)
        {
            int score = ScoreManager.Instance.Score;
            scoreText.text = "SCORE  : " + score.ToString();
            wonUI.SetActive(true);
            Time.timeScale = 0f;
        }

        if (isInsideNextStageTrigger && ctx.performed)
        {
            DontDestroyOnLoad(player);
            SceneManager.LoadScene("Level3 Boss");


        }
    }

    private void OnEnable()
    {
        interact.action.Enable();
        interact.action.performed += OnInteract;
    }

    private void OnDisable() { 
        interact.action.performed -= OnInteract; 
        interact.action.Disable();
    }
}

