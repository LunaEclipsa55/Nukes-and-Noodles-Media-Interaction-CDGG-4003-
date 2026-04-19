using UnityEngine;
using System.Collections;

public class ChargingBeetle : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    private Rigidbody2D rb;
    
    [Header("Attack")]
    public int damage = 2;
    //public float attackRange = 1.5f;

    [Header("Detection")]
    public float detectRange = 8f;
    //public float stopDistance = 1.5f;

    [Header("Charge Stuff")]
    public float chargeSpeed = 8f;
    public float chargeTime = 0.5f;
    public float windup = 0.6f;
    public float chargeCooldown = 1.5f; 

    private bool isCharging = false;
    private bool isCooling = false;
    private Vector2 direction;

    float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null){
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!player) return;
        if(isCharging || isCooling) return;

        float d = Vector2.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        //only start charging when player is close
        if(d > detectRange) return;

        if(attackTimer <= 0f)
        {
            StartCoroutine(Charge());
        }
    }

    public IEnumerator Charge()
    {
        isCooling = true;

        //save the direction to the player
        //so the beetle charges in charges in a straight line
        direction = (player.position - transform.position).normalized;

        //Face the player before charging
        FaceDirection(direction);

        Debug.Log($"{gameObject.name} is winding up");

        //Short pause so player can react
        yield return new WaitForSeconds(windup);

        isCharging = true;

        float timer = 0f;
        while(timer < chargeTime)
        {
            rb.linearVelocity = direction * chargeSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        //Stop after charge ends
        rb.linearVelocity = Vector2.zero;
        isCharging = false;

        Debug.Log($"{gameObject.name} finished charging.");

        //Cooldown before next charge
        yield return new WaitForSeconds(chargeCooldown);//temporary no extra wait here
        isCooling = false;
    }

    public void FaceDirection(Vector2 dir)
    {
        if(dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if(dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        
        Debug.Log($"{gameObject.name} is looking at  player");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isCharging) return;

        if(col.collider.CompareTag("Player"))
        {
            PlayerStats player = col.collider.GetComponentInParent<PlayerStats>();
            if(player != null)
            {
                player.TakeDamage(damage);
                player.Knockback(transform.position);
                Debug.Log($"{gameObject.name} hit player for {damage} damage");
            }
        }
        //stop charging when hitting a wall
        rb.linearVelocity = Vector2.zero;
        isCharging = false;
    }
}
