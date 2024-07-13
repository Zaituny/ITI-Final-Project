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
    bool isFacingRight = true;
    [SerializeField] Animator animator;
    bool hitting = false;
    [SerializeField] float DetectDist;
    public static int health = 100;
    private PlayerEvents playerEvents;

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
    }



    private void PlayerEvents_OnDamageTaken(object sender, PlayerEvents.OnDamageTakenEventArgs e)
    {
        if(health<=10)
        {
            health = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        health -= e.Damage;
        Debug.Log($"Player took {e.Damage} damage, current health = {health}!");
        // Add logic for handling player damage here, e.g., reducing health
    }

    void Update()
    {
        HandleMovement();
        FlipIfNeeded();
        if (hitting)
        {
            CheckForEnemies();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
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
            animator.SetBool("Hit", true);
            hitting = true;
        }

        if (context.canceled)
        {
            animator.SetBool("Hit", false);
            hitting = false;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
            animator.SetBool("Run", true);
        }
        else if (context.canceled)
        {
            animator.SetBool("Run", false);
        }
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(horizontal * Speed, rb.velocity.y);
    }
}
