using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //maybe health?
    public int health;
    public int healthMax = 100;

    private void OnEnable()
    {
        health = healthMax;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    public void TakeDamage(int amount)
    {
        Debug.Log("Player takes " + amount + " damage.");
        if (amount <= 0) return;
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
        Application.Quit();
    }
}
