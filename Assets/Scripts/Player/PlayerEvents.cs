using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{ 
    [SerializeField] Rigidbody2D rb;
    public event EventHandler<OnDamageTakenEventArgs> OnDamageTaken;
    public class OnDamageTakenEventArgs : EventArgs
    {
        public int Damage;
    }
    private void Update()
    {
       
    }
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Trap"))
        {
            OnDamageTaken?.Invoke(this,new OnDamageTakenEventArgs { Damage = 10 });
        }
    }
}
