using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;
    public float life = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        //player takes dmage here
        var stats = other.GetComponentInParent<PlayerStats>();
        if(stats != null) stats.TakeDamage(damage);

        Debug.Log("working...");

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
