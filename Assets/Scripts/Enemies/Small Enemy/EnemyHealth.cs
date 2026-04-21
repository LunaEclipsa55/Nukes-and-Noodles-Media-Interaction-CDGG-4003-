using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int Health;
    HealthBar healthBar;

    private void Awake()
    {
        healthBar =  GetComponentInChildren<HealthBar>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = maxHealth;
        healthBar.UpdateHealthBar(Health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        Health -= amount;
        healthBar.UpdateHealthBar(Health, maxHealth);
        if (Health < 0) Health = 0;

        if (Health <= 0) Die();
    }

    void Die()
    {
        ScoreManager.Instance.AddScore(10);
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} dies.");
    }
}
