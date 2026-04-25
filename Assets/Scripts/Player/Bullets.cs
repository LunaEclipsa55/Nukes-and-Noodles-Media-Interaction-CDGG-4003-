using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;
    public float life;

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

    }

    //for enemy helath maybe
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;

        // if (other.CompareTag("Player"))//enemy
        // {
        //     // Implement ability logic here, e.g., granting a power-up or triggering an effect
        //     Debug.Log("Player has entered the ability trigger!");
        // }

        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            
        if (enemyHealth != null)
        {
            hit = true;
            enemyHealth.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }

        if(other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            hit = true;
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction)
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction;
    }
}
