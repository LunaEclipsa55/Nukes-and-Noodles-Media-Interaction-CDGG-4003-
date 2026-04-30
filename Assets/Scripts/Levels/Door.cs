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
    
    public Transform spawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("WonDoor"))
        {
            isInsideTrigger = true;
        } else if (CompareTag("NextLevel"))
        {
            Debug.Log("Nextstage enter");

            isInsideNextStageTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (CompareTag("WonDoor"))
        {
            isInsideTrigger = false;
        }

        if (CompareTag("NextLevel"))
        {
            Debug.Log("Nextstage exit");
            isInsideNextStageTrigger = false;
        }
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        Debug.Log(ctx.action.name + " clicked");
        if (isInsideTrigger)
        {
            int score = ScoreManager.Instance.Score;
            scoreText.text = "SCORE  : " + score.ToString();
            wonUI.SetActive(true);
            Time.timeScale = 0f;
        } else if (isInsideNextStageTrigger)
        {
            Debug.Log("Nextstage clicked");

           
            player.transform.position = spawnPoint.position;


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

