using UnityEngine;

public class WhispyWasp : MonoBehaviour
{
    [Header("Target")]
    private Transform player;

    [Header("Weaponry")]
    public GameObject enemyBullet;
    public Transform firepoint;

    public Patrol patrol;

    public float detectRange = 5f;
    public float bulletSpeed = 7f;
    public float shootcool = 2f;

    float shootTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (patrol == null)
        {
            patrol = GetComponent<Patrol>();
        }
        if (player == null){
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!player) return;

        float d = Vector3.Distance(transform.position, player.position);
        shootTimer -= Time.deltaTime;

        // If player is too far, keep patrolling
        if (d > detectRange)
        {
            if (patrol != null)
                patrol.enabled = true;

            return;
        }

        // Player detected:
        // stop patrol so the wasp can face the player
        if (patrol != null)
            patrol.enabled = false;

        if(shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootcool;
        }
    }

    void Shoot()
    {
        if (!enemyBullet || !firepoint) return;

        GameObject bullet = Instantiate(enemyBullet, firepoint.position, firepoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0; // No gravity for bullets
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Use Continuous for fast-moving objects
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Smooth out movement
        Vector2 d = (player.position - firepoint.position).normalized;
        rb.linearVelocity = d * bulletSpeed;
    }
}

