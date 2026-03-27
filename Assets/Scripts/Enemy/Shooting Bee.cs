using UnityEngine;

public class ShootingBee : MonoBehaviour
{
    public Transform player;
    public GameObject enemyBullet;
    public Transform firepoint;
    public float bulletSpeed = 15f;
    public float shootcool = 2f;

    float shootTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null){
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!player) return;

        float d = Vector2.Distance(transform.position, player.position);
        shootTimer -= Time.deltaTime;

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
        if (rb) rb.linearVelocity = firepoint.forward * bulletSpeed;
    }
}
