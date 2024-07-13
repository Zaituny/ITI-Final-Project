using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrap : MonoBehaviour
{
    [SerializeField] float cooldown = 2f;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject dart;
    float cooldown_timer = 0f;

    void Attack()
    {
        cooldown_timer = 0;
        GameObject newDart = Instantiate(dart, firePoint.position, firePoint.rotation);
        newDart.GetComponent<Dart>().ActivateProjectile();
    }

    void Start()
    {
        cooldown_timer = cooldown; // Start with a ready-to-fire state
    }

    void Update()
    {
        cooldown_timer += Time.deltaTime;
        if (cooldown_timer > cooldown)
        {
            Attack();
        }
    }
}
