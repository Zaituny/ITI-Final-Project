using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{ 
    Rigidbody2D myKey;
    public int Keys = 0;
    
    
    private void Awake()
    {
        myKey = GetComponent<Rigidbody2D>();
        
    }
    void Start()
    {
        
    }

   
    void Update()
    {
        //Debug.Log(KeyCount);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            myKey.GetComponent<SpriteRenderer>().enabled = false;
            myKey.GetComponent<BoxCollider2D>().enabled = false;
            Keys++;
        }

    }
   
}
