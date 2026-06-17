using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;    // Speed of the trap's movement
    [SerializeField] private float moveDistance = 3.0f; // Distance the trap moves from its starting point

    private Vector3 startPos;
    private bool movingRight = true;

    private void Start()
    {
        // Store the starting position of the trap
        startPos = transform.position;
    }

    private void Update()
    {
        // Determine the movement direction
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            // Check if the trap has moved the specified distance to the right
            if (Vector3.Distance(startPos, transform.position) >= moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            // Check if the trap has moved the specified distance to the left
            if (Vector3.Distance(startPos, transform.position) >= moveDistance)
            {
                movingRight = true;
            }
        }
    }
}