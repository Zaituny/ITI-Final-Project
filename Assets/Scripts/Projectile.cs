using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private EnemyLevel3 parent;
    private Rigidbody2D rb;
    public float force;
    private Vector3 startPosition;
    private bool inFlight = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        if (!inFlight) {
            transform.position = parent.transform.position;
        }
        if (Vector2.Distance(transform.position, player.transform.position) <= 1f) {
            inFlight = false;
        }
    }
    public void Shoot() {
        inFlight = true;
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }
}
