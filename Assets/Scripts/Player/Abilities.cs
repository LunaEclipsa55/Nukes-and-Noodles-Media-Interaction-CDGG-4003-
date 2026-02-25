using UnityEngine;

public class Abilities : MonoBehaviour
{
    public int damage = 10;
    public float life = 5f;

    Rigidbody2D rb;
    bool hit;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Use Continuous for fast-moving objects
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Smooth out movement
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()//awake?
    {
        Destroy(gameObject, life);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Gun();
        }
    }

    void Gun()//maybe it own script instead of here?
    {

    }

    //for enemy helath maybe
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//enemy
        {
            // Implement ability logic here, e.g., granting a power-up or triggering an effect
            Debug.Log("Player has entered the ability trigger!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Implement ability logic here, e.g., granting a power-up or triggering an effect
            Debug.Log("Player has collided with the ability object!");
        }
    }
}
