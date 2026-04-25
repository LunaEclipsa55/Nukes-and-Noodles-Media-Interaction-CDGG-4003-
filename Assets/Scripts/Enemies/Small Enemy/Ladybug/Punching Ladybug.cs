using UnityEngine;

public class PunchingLadybug : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    
    [Header("Attack")]
    public int damage = 2;
    public float cooldown = 1.5f;
    public float attackRange = 1.5f;

    public Patrol patrol;

    float attackTimer;

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

        float d = Vector2.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        if(d > attackRange)
        {
            if(patrol != null)
            {
                patrol.enabled = true;
                return;
            }
        }

        if(patrol != null)
        {
            patrol.enabled = false;
        }

        FacePlayer();

        if(attackTimer <= 0f)
        {
            Punch();
            attackTimer = cooldown;
        }
    }

    void FacePlayer()
    {
        if (!player) return;

        Vector3 scale = transform.localScale;

        if (player.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x);   // face right
        else if (player.position.x < transform.position.x)
            scale.x = -Mathf.Abs(scale.x);  // face left

        transform.localScale = scale;
    }

    void Punch()
    {
        var stats = player.GetComponentInParent<PlayerStats>();
        if(stats != null) stats.TakeDamage(damage); Debug.Log($"{gameObject.name} punched player for {damage}");
    }
}
