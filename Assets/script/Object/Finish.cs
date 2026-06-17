using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    UIManager manager;
    [Header("SFX")]
    [SerializeField] AudioClip finish;
    private void Awake()
    {
        manager = FindFirstObjectByType<UIManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Trigger the finish UI through the UIManager
            GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();
            UnlockNewLevel();
            manager.Finish();
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(finish);
            }
        }
    }

    // Call this when a level is completed (e.g., after winning or reaching the end)
    public void UnlockNewLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int highestReachedLevel = PlayerPrefs.GetInt("ReachedIndex", 1); // Default: Level 1 unlocked

        // Only unlock the NEXT level if the current one is newly completed
        if (currentLevelIndex >= highestReachedLevel)
        {
            PlayerPrefs.SetInt("ReachedIndex", currentLevelIndex + 1);
            PlayerPrefs.Save();
            Debug.Log($"Unlocked Level: {currentLevelIndex + 1}"); // Optional log
        }
    }
}
