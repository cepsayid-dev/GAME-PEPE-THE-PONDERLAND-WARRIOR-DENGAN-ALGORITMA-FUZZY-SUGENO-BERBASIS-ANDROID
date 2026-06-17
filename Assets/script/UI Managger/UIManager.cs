using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip clickSfx;
    [Header("Panel")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject complete;
    [Header("Game Element")]
    [SerializeField] private GameObject control;
    [SerializeField] private GameObject heatlhbar;
    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pause.SetActive(false);
        complete.SetActive(false);
        settings.SetActive(false);
    }

    //activate game over screen
    public void GameOver () 
    {
        Time.timeScale = 0; // Paused the game 
        gameOverScreen.SetActive (true);
        control.SetActive(false);
        heatlhbar.SetActive(false);
    }
    //activate game over screen
    public void Finish()
    {
        Time.timeScale = 0; // Paused the game 
        complete.SetActive(true);
        control.SetActive(false);
        heatlhbar.SetActive(false);
    }

    public void Restart()
    {
        SoundManager.instance.PlaySound(clickSfx);
        Time.timeScale = 1; // Ensure the game runs at normal speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene

        // Use a coroutine to delay the execution until the scene is reloaded
        StartCoroutine(ResetPlayerMovement());
    }

    private IEnumerator ResetPlayerMovement()
    {
        // Wait for the scene to fully load
        yield return null;

        // Find the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Get the PlayerMovement component
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                // Disable the component
                movement.enabled = false;

                // Wait for 2 seconds
                yield return new WaitForSeconds(5f);

                // Re-enable the component
                movement.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning("Player not found in the scene after restart.");
        }
    }

    //main menu
    public void MainMenu() 
    {
        SoundManager.instance.PlaySound(clickSfx);
        SceneManager.LoadScene(0);
        Time.timeScale = 1; // Resumes the game if it was paused
    }

    //Pause
    public void Pause()
    {
        SoundManager.instance.PlaySound(clickSfx);
        pause.SetActive(true);
        control.SetActive(false);
        heatlhbar.SetActive(false);
        Time.timeScale = 0; // Paused the game 
    }

    //Settings
    public void Settings()
    {
        SoundManager.instance.PlaySound(clickSfx);
        settings.SetActive(true);
        control.SetActive(false);
        heatlhbar.SetActive(false);
    }

    //Back
    public void Back()
    {
        SoundManager.instance.PlaySound(clickSfx);
        settings.SetActive(false);
        pause.SetActive(true);
        control.SetActive(false);
        heatlhbar.SetActive(false);
    }

    //Quit
    public void Quit()
    {
        SoundManager.instance.PlaySound(clickSfx);
        Application.Quit();
    }

    //Continue
    public void Continue()
    {
        SoundManager.instance.PlaySound(clickSfx);
        pause.SetActive(false);
        control.SetActive(true);
        heatlhbar.SetActive(true);
        Time.timeScale = 1; // Resumes the game if it was paused
    }

    //Next Level
    public void NextLevel()
    {
        SoundManager.instance.PlaySound(clickSfx);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings) // Check if next level exists
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("No more levels to load."); // Handle case where there are no more levels
                                                  // Optionally, return to main menu or show a completion screen
            
        }
    }

    //Level Selection
    public void Level1()
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        try
        {
            SceneManager.LoadScene(1);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }

    public void Level2()
    {

        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        try
        {
            //SceneManager.LoadScene(2);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }

    public void Level3()
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        try
        {
            //SceneManager.LoadScene(3);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }

    public void Level4()
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        try
        {
            //SceneManager.LoadScene(4);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }

    public void Demo() 
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
            
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }
        try
        {
            SceneManager.LoadScene(6);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }
}
