using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{ 
    public Slider Slider;
    public Transform player; // Reference to the player's transform
    public Vector3 offset;
    void Start()
    {
        // Store the initial rotation of the health bar
        
    }

    void LateUpdate()
    {
        transform.position = player.position + offset;

    }
    public void SetMaxHealth(int maxHealth)
    {  Slider.maxValue = maxHealth; 
       Slider.value = maxHealth;
    }
    public void SetHealth(int health)
    {

    Slider.value = health; 
    }
    
    
}
