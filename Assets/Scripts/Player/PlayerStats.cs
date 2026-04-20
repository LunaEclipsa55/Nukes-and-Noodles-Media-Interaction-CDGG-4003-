using System;
using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int health;
    public int healthMax;
    public GameObject diedUI;
    
    public delegate void OnHealthChanged();
    public event OnHealthChanged onHealthChanged;

<<<<<<< HEAD
    public static bool isDead = false;
=======
    [Header("Knockback")]
    public float knockbackForce = 3f;
    public float knockbackTime = 0.2f;

    private bool isKnocked = false;
    private Rigidbody2D rb;

    [SerializeField] public SpriteRenderer playerSp;
    [SerializeField] public PlayerMovement move;
>>>>>>> Enemy

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        diedUI.SetActive(false);

        health = healthMax;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Awake()
    {
        diedUI.SetActive(false);
    }

    private void Update()
    {
        Debug.Log("Player is dead?"  + isDead);
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
        onHealthChanged?.Invoke();
        if (health <= 0)
        {
            Die();
        }
        
        
    }

    void Die()
    {
        if(isDead) return;
        
        Debug.Log("Ded.");
        isDead = true;
        diedUI.SetActive(true);
        Time.timeScale = 0f;
        
        //gameObject.SetActive(false);
    }
}
