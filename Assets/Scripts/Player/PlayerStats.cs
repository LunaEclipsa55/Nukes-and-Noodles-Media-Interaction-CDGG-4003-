using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //maybe health?
    public int health;
    public int healthMax;
    public GameObject diedUI;

    public static bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
<<<<<<< Updated upstream
=======
        diedUI.SetActive(false);
>>>>>>> Stashed changes
        health = healthMax;
    }

    // Update is called once per frame
    void Awake()
    {
        diedUI.SetActive(false);
    }

    private void Update()
    {
<<<<<<< Updated upstream
        if (health <= 0)
        {
            Die();
        }
=======
        Debug.Log("Player is dead?"  + isDead);
>>>>>>> Stashed changes
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
