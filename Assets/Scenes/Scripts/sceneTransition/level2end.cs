using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class level2end : MonoBehaviour
{
    [SerializeField] Sprite opened;
    [SerializeField] Sprite closed;
    [SerializeField] Key myKey;
    SpriteRenderer door;
   
    


    private void Awake()
    {
      
      door = GetComponent<SpriteRenderer>();
      
    }
    void Start()
    {
        door.sprite = closed;
    }

    
    void Update()
    {
      if(myKey.getkey() == 1)
        {
            door.sprite = opened;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && myKey.getkey() == 1)
        {
            SceneManager.LoadScene(4);
        }

    }
   
}
