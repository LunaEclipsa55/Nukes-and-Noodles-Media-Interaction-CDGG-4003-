using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    private bool isInsideTrigger = false;
    
    [Header("Input")]
    public InputActionReference interact;

    void OnTriggerEnter(Collider other)
    {
        isInsideTrigger = true;
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (isInsideTrigger && ctx.performed)
        {
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
