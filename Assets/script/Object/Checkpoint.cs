using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip checkpoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        PRespawn playerRespawn = collision.GetComponent<PRespawn>();
        if (playerRespawn != null)
        {
            // Set this checkpoint as the player's current checkpoint
            playerRespawn.SetCheckpoint(transform.position);
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(checkpoint);
            }
        }
    }
}
