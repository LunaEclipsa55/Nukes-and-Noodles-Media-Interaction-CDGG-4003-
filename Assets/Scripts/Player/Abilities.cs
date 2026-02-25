using UnityEngine;

public class Abilities : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()//awake?
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Gun()//maybe it own script instead of here?
    {

    }

    //for enemy helath maybe
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//enemy
        {
            // Implement ability logic here, e.g., granting a power-up or triggering an effect
            Debug.Log("Player has entered the ability trigger!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Implement ability logic here, e.g., granting a power-up or triggering an effect
            Debug.Log("Player has collided with the ability object!");
        }
    }
}
