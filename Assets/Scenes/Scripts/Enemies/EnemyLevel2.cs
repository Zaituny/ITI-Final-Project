using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevel2 : Enemy, IEnemy
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
    void Awake()
    {
        this.state = State.Patrol;
        this.rb = GetComponent<Rigidbody2D>();
        if (towardsRight)
        {
            direction = new Vector2(1, 0);

        }
        else
        {
            direction = new Vector2(-1, 0);
        }
        layerMask = 1 << MaskNumber;
        attackTimer = 0;
    }

    void FixedUpdate()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > 1)
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

    public void Patrol()
    {
        if (transform.position.x <= this.rightPatrolLimit.position.x && !towardsRight)
        {
            direction = -direction;
            Flip();
        }
        else if (transform.position.x >= this.leftPatrolLimit.position.x && towardsRight)
        {
            direction = -direction;
            Flip();
        }
        rb.AddForce(direction.normalized * 5);

    }
    public void Attack()
    {
        StartCoroutine(Dash());
        OnDamageDealt?.Invoke(this, new OnDamageDealtEventArgs { damage = 20 });
    }

    public void FollowPlayer()
    {
        Debug.Log(Vector2.Distance(transform.position, player.transform.position));
        if (Vector2.Distance(transform.position, player.transform.position) < 8f)
        {
            if (attackTimer == 0f)
            {
                Attack();
            }
        }
        else if (Vector2.Distance(transform.position, this.rightPatrolLimit.position) > rightMaxFollowDistance &&
            transform.position.x < rightPatrolLimit.position.x)
        {
            this.state = State.ReturningToPatrol;
        }
        else if (Vector2.Distance(transform.position, this.leftPatrolLimit.position) > leftMaxFollowDistance &&
            transform.position.x > leftPatrolLimit.position.x)
        {
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
        else
        {
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
        if (IsInFOV(collision.transform.position))
        {
            Debug.Log("in fov");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, collision.transform.position - transform.position, 10f, layerMask);
            if (hit.transform)
            {
                Debug.Log(hit.transform);
                if (hit.transform.gameObject.TryGetComponent<Player>(out player))
                {
                    Debug.Log("we hit a player");
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

    private IEnumerator Dash() {
        float originalGravity = rb.gravityScale;
        Vector2 originalVelocity = rb.velocity;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(-transform.localScale.x * 15f, 0f);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector2(transform.localScale.x * 15f, 0f);
        yield return new WaitForSeconds(0.2f);
        rb.gravityScale = originalGravity;
        rb.velocity = originalVelocity;
        yield return new WaitForSeconds(1f);


    }
}
