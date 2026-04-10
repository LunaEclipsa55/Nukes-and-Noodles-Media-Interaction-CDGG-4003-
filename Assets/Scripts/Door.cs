using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    private bool isInsideTrigger = false;
    
    [Header("Input")]
    public InputActionReference interact;

    [Header("UI")]
    public GameObject wonUI;
    public Text scoreText;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name + " entered");
        isInsideTrigger = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        isInsideTrigger = false;
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.action.name + " clicked");
        if (isInsideTrigger && ctx.performed)
        {
            int score = ScoreManager.score;
            scoreText.text = "SCORE  : " + score.ToString();
            wonUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    
    private void OnEnable()
    {
        interact.action.Enable();
        interact.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        interact.action.performed -= OnInteract;
        interact.action.Disable();
    }
        
}
