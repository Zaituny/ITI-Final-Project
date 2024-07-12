using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes : MonoBehaviour
{
    Rigidbody2D myspike;
    float damage = 0;
    private void Awake()
    {
        myspike = GetComponent<Rigidbody2D>();  
    }

    void Start()
    {
        
    }

    
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("damage");


        }

    }
}
