using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level2end : MonoBehaviour
{
    [SerializeField] Sprite opened;
    [SerializeField] Sprite closed;
    SpriteRenderer door;
    Key myKey;
    


    private void Awake()
    {
      
      door = GetComponent<SpriteRenderer>();
      myKey = GetComponent<Key>();
    }
    void Start()
    {
        door.sprite = closed;
    }

    
    void Update()
    {
      /*if(myKey.Keys == 3)
        {
            door.sprite = opened;
        }*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") /*&& myKey.Keys == 3*/)
        {
            SceneManager.LoadScene(4);
        }

    }
   
}
