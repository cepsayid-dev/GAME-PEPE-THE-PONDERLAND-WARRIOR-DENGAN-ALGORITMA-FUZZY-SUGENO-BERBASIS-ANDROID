using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private Animator anim;
    private PolygonCollider2D polCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private bool isTouchingLeftWall, isTouchingRightWall;

    private void Awake()
    {
        // Grab references for Rigidbody and Animator from the object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        polCollider = GetComponent<PolygonCollider2D>();

    }

    private void Update()
    {
        // Detect wall collisions
        DetectWallCollisions();

        // Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Set animator parameters
        anim.SetBool("walk", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Apply movement if no wall is blocking in the direction of movement
        if (wallJumpCooldown > 0.2f)
        {
            if ((!isTouchingLeftWall || horizontalInput > 0) && (!isTouchingRightWall || horizontalInput < 0))
            {
                body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
            }
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
    }

    private void DetectWallCollisions()
    {
        // Left wall detection
        Vector2 leftPosition = new Vector2(polCollider.bounds.min.x, polCollider.bounds.center.y);
        RaycastHit2D leftHit = Physics2D.Raycast(leftPosition, Vector2.left, 0.1f, groundLayer);
        isTouchingLeftWall = leftHit.collider != null;

        // Right wall detection
        Vector2 rightPosition = new Vector2(polCollider.bounds.max.x, polCollider.bounds.center.y);
        RaycastHit2D rightHit = Physics2D.Raycast(rightPosition, Vector2.right, 0.1f, groundLayer);
        isTouchingRightWall = rightHit.collider != null;
    }

    private bool isGrounded()
    {
        // Cast rays down from the left and right edges of the collider for ground detection
        Vector2 leftEdge = new Vector2(polCollider.bounds.min.x, polCollider.bounds.min.y);
        Vector2 rightEdge = new Vector2(polCollider.bounds.max.x, polCollider.bounds.min.y);
        RaycastHit2D leftHit = Physics2D.Raycast(leftEdge, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rightEdge, Vector2.down, 0.1f, groundLayer);

        return leftHit.collider != null || rightHit.collider != null;
    }

}