using System.Collections;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

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

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(CaptureLaser()); 
            CaptureCollider();
        } 
       


    }

    void CaptureCollider()
    {
        int objectsHit = playerCollider.Raycast(Vector2.right, hits,maxDistance);

        for (int i = 0; i < objectsHit; i++)
        {
            Debug.Log("Captured " + hits[i].collider.name);
            if (hits[i].collider.CompareTag("Enemy"))
            {
                if (hits[i].collider.GetComponent<BeeEnemy>())
                {
                    BeeEnemy beeAbility = hits[i].collider.GetComponent<BeeEnemy>();
                        
                    var inv = Inventory.Instance;
                    if (!inv) return; 


                    bool added = inv.AddToInventory(beeAbility.initialAmount, beeAbility.bulletName);
                    if (added)
                    {
                        Destroy(hits[i].transform.gameObject);
                    }
        
                }

                    
            }
        }
             
    }

    IEnumerator CaptureLaser()
    {
        lineRenderer.enabled = true; 

        yield return new WaitForSeconds(0.5f);

        lineRenderer.enabled = false;
    }
}
