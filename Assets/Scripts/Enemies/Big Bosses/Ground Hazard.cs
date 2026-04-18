using UnityEngine;

public class GroundHazard : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only damage the player
        if (!other.CompareTag("Player"))
            return;

        // Find PlayerStats on the player or parent object
        PlayerStats player = other.GetComponentInParent<PlayerStats>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log("Player touched hazard and took damage.");
        }
    }
}
