using UnityEngine;

public class WarningCleanup : MonoBehaviour
{
    public float life = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, life);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
