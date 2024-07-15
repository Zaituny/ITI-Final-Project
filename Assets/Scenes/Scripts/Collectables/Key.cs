using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{ 
    Rigidbody2D myKey;
    int Keys;
    
    
    private void Awake()
    {
        myKey = GetComponent<Rigidbody2D>();
        
    }
    void Start()
    {
        Keys = 0;
    }

   
    void Update()
    {
        Debug.Log(Keys);
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
    public int getkey()
    {
        return Keys;
    }
}
