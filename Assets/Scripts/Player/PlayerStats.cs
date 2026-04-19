using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int health;
    public int healthMax = 3;
    [SerializeField]private GameObject diedUI;

    [Header("Knockback")]
    public float knockbackForce = 3f;
    public float knockbackTime = 0.2f;

    private bool isKnocked = false;
    private Rigidbody2D rb;

    [SerializeField] public SpriteRenderer playerSp;
    [SerializeField] public PlayerMovement move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //diedUI = GameObject.Find("DiedUI");
        //diedUI.SetActive(false);
        health = healthMax;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        health += Mathf.Min(health + amount, healthMax);
    }

    public void ApplyKnockback(Vector2 pos)
    {
        if(rb == null) return;

        Vector2 d = (transform.position - (Vector3)pos).normalized;

        StartCoroutine(Knockback(d));
    }

    public IEnumerator Knockback(Vector2 lol)
    {
        isKnocked = true;

        float timer = 0f;
        while(timer < knockbackTime)
        {
            rb.linearVelocity = lol * knockbackForce;
            timer += Time.deltaTime;
            yield return null;
        }
        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Player takes " + amount + " damage.");
        if (amount <= 0) return;
        health -= amount;
        if (health <= 0)
        {
            playerSp.enabled = false;
            move.enabled = false;
            //diedUI.SetActive(true);
            Debug.Log("Ded.");
        }
    }
}
