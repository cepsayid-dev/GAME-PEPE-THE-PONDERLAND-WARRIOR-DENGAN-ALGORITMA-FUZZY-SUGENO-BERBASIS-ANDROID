using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;           // Speed at which the spike falls
    [SerializeField] private int damageAmount = 20;          // Damage inflicted on the player when hit
    [SerializeField] private LayerMask detectionLayer;       // LayerMask to detect both player and ground
    [SerializeField] private float detectionRadius = 5f;     // Radius to check for player or ground

    private bool isFalling = false;

    private void Start()
    {
        // Ensure the spike has a Rigidbody2D component
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic; // Use kinematic so we can control movement
        }

        // Optional: Set the Collision Detection to Continuous
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private void Update()
    {
        // Only start falling if the player is under the spike
        if (!isFalling && IsPlayerOrGroundBelow())
        {
            StartFalling();
        }

        // If the spike is falling, move it downwards
        if (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    private bool IsPlayerOrGroundBelow()
    {
        // Raycast downward from the spike's position to check for player or ground below
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionRadius, detectionLayer);

        // Visualize the ray in the Scene view
        Debug.DrawRay(transform.position, Vector2.down * detectionRadius, Color.red);

        // Debug: Log the Raycast results
        if (hit.collider != null)
        {
            Debug.Log("Hit detected: " + hit.collider.gameObject.name);
        }

        // Return true if the raycast hits either the player or the ground
        return hit.collider != null;
    }

    private void StartFalling()
    {
        // Start falling when the player is detected below the spike
        isFalling = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug: Check the collision object
        Debug.Log("Spike collided with: " + collision.gameObject.name);

        // If the spike hits the player, apply damage and stop
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply damage to the player
            Damageable damageable = collision.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
            }

            StopAndDestroy();
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            // If the spike hits the ground, stop and destroy it
            StopAndDestroy();
        }
    }

    private void StopAndDestroy()
    {
        // Stop the spike's movement and destroy it
        isFalling = false;
        Destroy(gameObject); // Destroy the spike object after it hits the ground or player
    }
}