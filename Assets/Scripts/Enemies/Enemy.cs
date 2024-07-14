using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public event EventHandler<OnDamageDealtEventArgs> OnDamageDealt;
    public class OnDamageDealtEventArgs : EventArgs
    {
        public int damage;
    }
    [SerializeField] public Transform rightPatrolLimit;
    [SerializeField] public Transform leftPatrolLimit;
    [SerializeField] public int rightMaxFollowDistance;
    [SerializeField] public int leftMaxFollowDistance;
    [SerializeField] public int FOVangle;

    public Rigidbody2D rb;
    public enum State
    {
        Patrol,
        Follow,
        ReturningToPatrol
    }

    public State state;
}
