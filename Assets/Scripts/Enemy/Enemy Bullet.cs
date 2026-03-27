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
        if(other.CompareTag("Player"))
        {
            //player takes dmage here
            var stats = other.GetComponent<PlayerStats>();
            if(!stats) stats = other.GetComponentInParent<PlayerStats>();
            if(!stats) stats.TakeDamage(damage);
        }

        if(!other.isTrigger) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
