using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int Health = 100;
    public bool death = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = Mathf.Clamp(Health, 0, maxHealth)
        if (Health == 0) Health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        Health -= amount;
        if (Health < 0) Health = 0;

        Debug.log($"{gameObject.name} took {amount} damage.")

        if (Health <= 0) Die();
    }

    void Die()
    {
        if(death) Destroy(gameObject);
        debug.log($"{gameObject.name} dies.")
    }
}
