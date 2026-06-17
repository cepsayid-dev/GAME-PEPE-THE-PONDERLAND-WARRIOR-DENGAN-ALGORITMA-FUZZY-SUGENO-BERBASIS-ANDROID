using System.Collections;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;        // Speed of the platform's movement
    [SerializeField] private float moveDistance = 5.0f;     // Distance the platform moves up from its starting point
    [SerializeField] private float pauseDuration = 1.0f;    // Duration the platform pauses at the top

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingUp = true;
    private bool isPaused = false;

    private void Start()
    {
        // Store the initial position of the platform
        startPos = transform.position;

        // Calculate the end position based on the move distance
        endPos = startPos + Vector3.up * moveDistance;
    }

    private void Update()
    {
        // If the platform is paused, do nothing
        if (isPaused) return;

        // Move the platform up or down
        if (movingUp)
        {
            // Move the platform towards the end position
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);

            // Check if the platform has reached the end position
            if (transform.position == endPos)
            {
                movingUp = false;
                StartCoroutine(PauseAtTop());
            }
        }
        else
        {
            // Move the platform back towards the start position
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);

            // Check if the platform has returned to the start position
            if (transform.position == startPos)
            {
                StartCoroutine(PauseAtTop());
                movingUp = true;
            }
        }
    }

    private IEnumerator PauseAtTop()
    {
        // Pause the platform at the top for the specified duration
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        isPaused = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is colliding with the platform
        if (collision.collider.CompareTag("Player"))
        {
            // Make the player a child of the platform
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player is leaving the platform
        if (collision.collider.CompareTag("Player"))
        {
            // Remove the player from the platform's hierarchy
            collision.transform.SetParent(null);
        }
    }
}