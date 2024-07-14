using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform front;
    public LayerMask groundLayer;
    float horizontal;
    [SerializeField] float Speed;
    [SerializeField] float jumpingPower;
    [SerializeField] float groundDist=0.2f;
    bool isFacingRight = true;
    [SerializeField] Animator animator;
    bool hitting = false;
    [SerializeField] float DetectDist;
    public static int health = 100;
    private PlayerEvents playerEvents;
    public HealthBar healthBar;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private EnemyLevel1 el1;
    [SerializeField] private EnemyLevel2 el2;
    [SerializeField] private EnemyLevel3 el3;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerEvents = GetComponent<PlayerEvents>();
    }

    private void Start()
    {
        if (playerEvents != null)
        {
            playerEvents.OnDamageTaken += PlayerEvents_OnDamageTaken;
        }
        el1.OnDamageDealt += E1_DamageDealt;
        el2.OnDamageDealt += E2_DamageDealt;
        el3.OnDamageDealt += E3_DamageDealt;
        healthBar.SetMaxHealth(health);
    }

    private void E1_DamageDealt(object sender, EnemyLevel1.OnDamageDealtEventArgs e) {
        if (health < 10)
        {
            animator.SetTrigger("Death");
            health = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        health -= e.damage;
        animator.SetTrigger("Hurt");
    }

    private void E2_DamageDealt(object sender, EnemyLevel2.OnDamageDealtEventArgs e)
    {
        if (health < 10)
        {
            animator.SetTrigger("Death");
            health = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        health -= e.damage;
        animator.SetTrigger("Hurt");
    }

    private void E3_DamageDealt(object sender, EnemyLevel3.OnDamageDealtEventArgs e)
    {
        if (health < 10)
        {
            animator.SetTrigger("Death");
            health = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        health -= e.damage;
        animator.SetTrigger("Hurt");
    }

    private void PlayerEvents_OnDamageTaken(object sender, PlayerEvents.OnDamageTakenEventArgs e)
    {
        if(health<10)
        {
            animator.SetTrigger("Death");
            health = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        health -= e.Damage;
        animator.SetTrigger("Hurt");
    }
    
    void Update()
    {
        animator.SetBool("Grounded", IsGrounded());
        animator.SetFloat("AirSpeed",rb.velocity.y);
        HandleMovement();
        FlipIfNeeded();
        if (hitting)
        {
            CheckForEnemies();
        }
        healthBar.SetHealth(health);
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        
        if (context.performed && IsGrounded() && rb.velocity.y == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");

        }
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            
        }
        

    }
    
    public void Hit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Attack");
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            RaycastHit2D hit = Physics2D.Raycast(front.position, direction, DetectDist, LayerMask.GetMask("OPPS"));
            if (hit.transform) {
                if (hit.transform.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
                    if (enemy.gameObject.TryGetComponent<Health>(out Health health)) {
                        health.TakeDamage(50);
                        Debug.Log(health.health);
                    }
                }
            }
        }

        
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, groundDist , groundLayer);
    }

    void FlipIfNeeded()
    {
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector2 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void CheckForEnemies()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(front.position, direction, DetectDist, LayerMask.GetMask("OPPS"));

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Detected object with tag: Enemy");
        }
        else
        {
            Debug.Log("No object detected or tag does not match.");
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        if (context.performed)
        {
            animator.SetInteger("AnimState", 2);
        }
        else if (context.canceled)
        {
            animator.SetInteger("AnimState", 0);
        }
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(horizontal * Speed, rb.velocity.y);
    }
}
