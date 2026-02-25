using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage = 10;
    public float life = 5f;

    Rigidbody2D rb;
    bool hit;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // No gravity for bullets
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;
        hit = true;

        if (other.CompareTag("Player"))//enemy
        {
            // Implement ability logic here, e.g., granting a power-up or triggering an effect
            Debug.Log("Player has entered the ability trigger!");
        }

        if (other.CompareTag("Enemy"))
        {
            //var enemyHealth = other.GetComponentInParent<EnemyHealth>();
            //if (enemyHealth != null)
            //{
            //    enemyHealth.TakeDamage(damage);
            //}

            Destroy(gameObject);
        }
        else if (other.CompareTag("Friend"))
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction)
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction;
    }
}
