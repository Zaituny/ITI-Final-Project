using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float resetTime = 5f;
    float lifetime = 0f;

    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true);
        
    }

    void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); 
        }
    }
    
}
