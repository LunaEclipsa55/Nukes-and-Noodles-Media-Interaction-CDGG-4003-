using UnityEngine;

public class PickUp : MonoBehaviour
{
    public string itemName = "Health10";
    public int amount = 1;
    public bool destroyOnPickup = true;

    void Reset()  // auto-fix common setup
    {
        var col = GetComponent<Collider2D>() ?? gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        var rb = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // required so triggers fire with CharacterController players
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = Inventory.Instance;
        if (!inv) return; 


        bool added = inv.AddToInventory(amount, itemName);
        if (added && destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
