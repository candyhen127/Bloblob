using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpHeight = 3f;         // max jump height in Unity units
    public float coyoteTime = 0.5f;       // grace period after leaving ground
    public float jumpCutMultiplier = 0.5f; // short hop multiplier

    [Header("Gravity")]
    public float fallMultiplier = 4f;    // faster falling
    // rising gravity stays normal (1f) for predictable jump height

    [Header("Checks")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.05f;
    public LayerMask groundLayer;
    public LayerMask blobLayer;
    public Transform wallCheck;
    public float wallCheckDistance = 0.05f;

    private Rigidbody2D rb;
    private float move;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private bool touchingWall;

    private float jumpForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // calculate jumpForce
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        jumpForce = Mathf.Sqrt(2 * gravity * jumpHeight);
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        // ground check
        isGrounded = (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, blobLayer));

        // update coyote time counter
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // wall check (both sides)
        touchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, groundLayer)
                    || Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, groundLayer);

        // jump input
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);
        bool jumpReleased = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W);

        if (jumpPressed && coyoteTimeCounter > 0f && rb.linearVelocity.y == 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0f;
        }

        // Short hop if jump released early
        if (jumpReleased && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        // Variable gravity for fast fall
        if (rb.linearVelocity.y < 0f) // falling
            rb.gravityScale = fallMultiplier;
        else // rising
            rb.gravityScale = 2f; // normal gravity for predictable jump height
    }

    void FixedUpdate()
    {
        // horizontal
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // prevent corner boost / sticking to walls
        if (!isGrounded && touchingWall && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }
}
