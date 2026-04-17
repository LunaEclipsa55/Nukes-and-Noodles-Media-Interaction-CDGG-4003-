using UnityEngine;

public class ChargingBeetle : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    
    [Header("Attack")]
    public int damage = 2;
    public float cooldown = 1.5f;
    public float attackRange = 1.5f;

    float attackTimer;

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
        attackTimer -= Time.deltaTime;

        if(d > attackRange) return;

        if(d <= attackRange && attackTimer <= 0f)
        {
            Charge();
            attackTimer = cooldown;
        }
    }

    void Charge()
    {
        var stats = player.GetComponentInParent<PlayerStats>();
        if(stats != null) stats.TakeDamage(damage); Debug.Log($"{gameObject.name} punched player for {damage}");
    }
}
