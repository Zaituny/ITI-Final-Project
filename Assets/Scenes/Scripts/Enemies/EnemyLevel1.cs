using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyLevel1 :Enemy, IEnemy
{
    public event EventHandler<OnDamageDealtEventArgs> OnDamageDealt;
    public class OnDamageDealtEventArgs : EventArgs
    {
        public int damage;
    }
    public int MaskNumber;
    [SerializeField] private bool towardsRight;
    int layerMask;
    Vector2 direction;
    Player player;
    float attackTimer;
    void Awake() {
        this.state = State.Patrol;
        this.rb = GetComponent<Rigidbody2D>();
        if (towardsRight)
        {
            direction = new Vector2(1, 0);

        }
        else {
            direction = new Vector2(-1, 0);
        }
        layerMask = 1 << MaskNumber;
        attackTimer = 0;
    }
    
    void FixedUpdate()
    {
        Debug.Log(state);
        attackTimer += Time.deltaTime;
        if (attackTimer > 2)
        {
            attackTimer = 0f;
        }
        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Follow:
                FollowPlayer();
                break;
            case State.ReturningToPatrol:
                ReturnToPatrol();
                break;
        }
    }

    public void Patrol() {
        if (transform.position.x <= this.rightPatrolLimit.position.x && !towardsRight)
        {
            direction = -direction;
            Flip();
        }
        else if (transform.position.x >= this.leftPatrolLimit.position.x && towardsRight) {
            direction = -direction;
            Flip();
        }
        rb.AddForce(direction.normalized * 5);

    }
    public void Attack() {
        OnDamageDealt?.Invoke(this, new OnDamageDealtEventArgs { damage = 10 });
    }

    public void FollowPlayer() {

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y)) <= 1.5f) {
            if (attackTimer == 0f) {
                Attack();
            }
        }
        else if (Vector2.Distance(transform.position, this.rightPatrolLimit.position) > rightMaxFollowDistance &&
            transform.position.x < rightPatrolLimit.position.x)
        {
            this.state = State.ReturningToPatrol;
        }
        else if (Vector2.Distance(transform.position, this.leftPatrolLimit.position) > leftMaxFollowDistance &&
            transform.position.x > leftPatrolLimit.position.x) {
            this.state = State.ReturningToPatrol;
        }
        direction = (player.transform.position - transform.position).normalized;
        rb.AddForce(direction * 6f);
    }

    public void ReturnToPatrol()
    {
        if (transform.position.x < rightPatrolLimit.position.x)
        {
            direction = (rightPatrolLimit.position - transform.position).normalized;
            if (!towardsRight) Flip();
        }
        else if (transform.position.x > leftPatrolLimit.position.x)
        {
            direction = (leftPatrolLimit.position - transform.position).normalized;
            if (towardsRight) Flip();
        }
        else {
            state = State.Patrol;
            return;
        }
        rb.AddForce(direction * 5);
    }
    public void Flip()
    {
        Vector3 _scale = transform.localScale;
        _scale.x *= -1;
        transform.localScale = _scale;
        towardsRight = !towardsRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInFOV(collision.transform.position)){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.transform.position - transform.position, 7f, layerMask);
            if (hit.transform) {
                if (hit.transform.gameObject.TryGetComponent<Player>(out player)) {
                    this.state = State.Follow;
                }
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state == State.Follow)
        {
            state = State.ReturningToPatrol;
            
        }
    }

    private bool IsInFOV(Vector2 pos)
    {
        Vector2 to = pos - new Vector2(transform.position.x, transform.position.y);

        Vector2 test = -transform.right * (transform.localScale.x / Mathf.Abs(transform.localScale.x));
        //Debug.Log(test);
        float angle = Vector2.SignedAngle(test, to);

        if (angle < this.FOVangle / 2 && angle > -this.FOVangle / 2)
        {
            Debug.Log("in view");
            return true;
        }
        //Debug.Log(angle);
        return false;
    }
}
