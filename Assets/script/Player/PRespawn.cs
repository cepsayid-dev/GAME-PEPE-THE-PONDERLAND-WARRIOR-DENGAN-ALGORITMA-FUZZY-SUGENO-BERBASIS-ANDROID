using TMPro.Examples;
using UnityEngine;

public class PRespawn : MonoBehaviour
{
    private Vector3 checkpointPosition;
    private bool hasCheckpoint = false;
    private Damageable damageable;
    private UIManager manager;

    [SerializeField] private float respawnDelay = 2f; // Delay before respawn
    [SerializeField] private Behaviour[] components;
    [Header("SFX")]
    [SerializeField] AudioClip faildSfx;
    [SerializeField] AudioClip resSfx;

    private int checkpoint_chance = 0;

    private void Awake()
    {
        manager = FindFirstObjectByType<UIManager>();
    }
    private void Start()
    {
        damageable = GetComponent<Damageable>();
        // No checkpoint by default
        hasCheckpoint = false;
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        if (checkpointPosition == newCheckpoint) return;
        checkpoint_chance = 1;
        checkpointPosition = newCheckpoint;
        hasCheckpoint = true;
    }

    public void Respawn()
    {
        if (hasCheckpoint && checkpoint_chance > 0)
        {
            
            StartCoroutine(RespawnCoroutine());
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(resSfx);
            }

        }
        else
        {
            GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();
            manager.GameOver();
            if(SoundManager.instance != null) 
            { 
                SoundManager.instance.PlaySound(faildSfx); 
            }
        }
    }

    private System.Collections.IEnumerator RespawnCoroutine()
    {
        checkpoint_chance--;

        GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();

        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnDelay);

        GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Play();

        // Move the player to the checkpoint
        transform.position = checkpointPosition;

        // Restore player health and state
        damageable.Health = damageable.MaxHealth;
        damageable.IsAlive = true;

        foreach (Behaviour componment in components) componment.enabled = true;


        //Debug.Log("Player respawned at checkpoint!");
    }
}
