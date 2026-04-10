using System.Collections;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Capture : MonoBehaviour
{
    private Ray2D ray;
    [SerializeField] private int maxDistance; 
    public Collider2D playerCollider;
    RaycastHit2D[] hits = new RaycastHit2D[10];

    //Showing the line where it hits
    private LineRenderer lineRenderer;
    public Transform startLaser;
    public Transform endLaser;

    public ScoreManager scoreManager;
    
    [Header("Input")]
    [SerializeField] private InputActionReference catcher;
    bool laserOn = false;
    
    private void OnEnable()
    {
        catcher.action.Enable();
        catcher.action.performed += OnCatch;
    }

    private void OnDisable()
    {
        

        catcher.action.performed -= OnCatch;
        catcher.action.Disable();
    }

    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.positionCount = 2; 
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true; 
    }



    void Update()
    {
        lineRenderer.SetPosition(0, startLaser.position);
        lineRenderer.SetPosition(1, endLaser.position);

        // if (laserOn && !catcher.action.triggered)
        // {
        //     lineRenderer.enabled = false; 
        // }

        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     CaptureLaser();
        //     CaptureCollider();
        // } 
        //


    }

    void CaptureCollider()
    {
        int objectsHit;
        if (PlayerMovement.isFacingRight == true)
        {
            objectsHit = playerCollider.Raycast( Vector2.right, hits, maxDistance);

        }
        else
        {
            objectsHit = playerCollider.Raycast( Vector2.left, hits, maxDistance);

        }

        for (int i = 0; i < objectsHit; i++)
        {
            Debug.Log("Captured " + hits[i].collider.name);
            if (hits[i].collider.CompareTag("Enemy"))
            {
                if (hits[i].collider.GetComponent<BeeEnemy>())
                {
                    Debug.DrawRay(transform.position, Vector2.right * maxDistance, Color.red);

                    BeeEnemy beeAbility = hits[i].collider.GetComponent<BeeEnemy>();
                        
                    var inv = Inventory.Instance;
                    if (!inv) return; 


                    bool added = inv.AddToInventory(beeAbility.initialAmount, beeAbility.bulletName);
                    inv.UseItem(beeAbility.bulletName, beeAbility.initialAmount);
                    // inv.SetQuickItem()
                    if (added)
                    {
                        scoreManager.AddScore(10);
                        Destroy(hits[i].transform.gameObject);
                    }
        
                }
            }
        }
    }

    void CaptureLaser()
    {
        lineRenderer.enabled = true; 

        
    }
    
    private void OnCatch(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnCatch");
        if (ctx.performed)
        {
            
            // CaptureLaser();
            laserOn = true;
            CaptureCollider();

        }
    }
}
