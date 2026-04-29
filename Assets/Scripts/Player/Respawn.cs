using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.transform.position = spawnPoint.position;
    }
}
