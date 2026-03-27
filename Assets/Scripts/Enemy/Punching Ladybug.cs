using UnityEngine;

public class PunchingLadybug : MonoBehaviour
{
    [Header("Target")]
    public Trasnform player;
    
    [Header("Attack")]
    public int damage = 15;
    public flaot cooldown = 1.5f;

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

        float d = Vector3.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        if(attackTimer <= 0f)
        {
            Punch();
            attackTimer = cooldown;
        }
    }

    void Punch()
    {
        var stats = player.GetComponent<PlayerStats>();
            if(!stats) stats = player.GetComponentInParent<PlayerStats>();
            if(!stats) stats.TakeDamage(damage), Debug.Log($"{gameObject.name} punched player for {damage}");
    }
}
