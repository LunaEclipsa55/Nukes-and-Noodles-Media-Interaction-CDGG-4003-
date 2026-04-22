using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int Health;
    HealthBar healthBar;
    public AudioClip deathSound;
    private AudioSource audioSource;
    private void Awake()
    {
        healthBar =  GetComponentInChildren<HealthBar>();
        audioSource = GetComponent<AudioSource>();

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

        if (deathSound != null)
        {
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = transform.position;

            AudioSource aSource = tempAudio.AddComponent<AudioSource>();
            aSource.clip = deathSound;

            // Randomize pitch
            aSource.pitch = Random.Range(0.8f, 1.2f);

            aSource.Play();

            Destroy(tempAudio, deathSound.length);
        }

        Debug.Log($"{gameObject.name} dies.");

        Destroy(gameObject);
    }
}
