using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    [SerializeField]public PlayerStats playerHealth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth.onHealthChanged += UpdateHearts;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            hearts[i].sprite = (i < playerHealth.health) ? fullHeart : emptyHeart;
            hearts[i].enabled = (i < playerHealth.healthMax);
        }
    }



    /* void Update()
     {
         health = playerHealth.health;
         maxHealth = playerHealth.healthMax;
         for(int i = 0; i < hearts.Length; i++)
         {
             if (hearts[i] == null) continue;

             if(i < health)
             {
                 hearts[i].sprite = fullHeart;
             }
             else
             {
                 hearts[i].sprite = emptyHeart;
             }
             if(i < maxHealth)
             {
                 hearts[i].enabled = true;
             }
             else
             {
                 hearts[i].enabled = false;
             }
         }
     } */
}
