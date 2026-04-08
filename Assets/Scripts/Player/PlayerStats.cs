using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //maybe health?
    public int health;
    public int healthMax = 3;
    [SerializeField]private GameObject diedUI;

    [SerializeField] public SpriteRenderer playerSp;
    [SerializeField] public PlayerMovement move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //diedUI = GameObject.Find("DiedUI");
        //diedUI.SetActive(false);
        health = healthMax;
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
            playerSp.enabled = false;
            move.enabled = false;
            //diedUI.SetActive(true);
            Debug.Log("Ded.");
        }
    }
}
