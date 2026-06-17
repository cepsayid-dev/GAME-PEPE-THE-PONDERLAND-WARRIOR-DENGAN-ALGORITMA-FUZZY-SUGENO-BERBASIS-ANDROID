using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip clickSfx;
    public Button[] levelButtons;
    public GameObject buttonLevels;

    private void Awake()
    {
        ButtonsToArray();
        // Initialize all buttons as locked
        foreach (Button button in levelButtons)
        {
            button.interactable = false;
        }

        // Unlock levels up to the highest reached
        int highestUnlockedLevel = PlayerPrefs.GetInt("ReachedIndex", 1);
        for (int i = 0; i < highestUnlockedLevel && i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    private void ButtonsToArray()
    {
        int childCount = buttonLevels.transform.childCount;
        levelButtons = new Button[childCount];
        for (int i = 0; i < childCount; i++) 
        {
            levelButtons[i] = buttonLevels.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }

    public void OpenLevel(int levelId)
    {
        // Play SFX (optional)
        if (clickSfx != null)
        {
            AudioSource.PlayClipAtPoint(clickSfx, Camera.main.transform.position);
        }

        // Check if the level exists in Build Settings
        if (levelId < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelId);
        }
        else
        {
            Debug.LogError($"Level {levelId} does not exist in Build Settings!");
            // Optional: Load a fallback scene (e.g., Main Menu)
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Reset progress (for testing)
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Refresh UI
    }


    ////Level Selection
    //public void Level1()
    //{
    //    if (SoundManager.instance != null && clickSfx != null)
    //    {
    //        SoundManager.instance.PlaySound(clickSfx);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("SoundManager instance or clickSfx is null.");
    //    }

    //    try
    //    {
    //        SceneManager.LoadScene(1);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError($"Failed to load scene: {ex.Message}");
    //    }
    //}

    //public void Level2()
    //{

    //    if (SoundManager.instance != null && clickSfx != null)
    //    {
    //        SoundManager.instance.PlaySound(clickSfx);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("SoundManager instance or clickSfx is null.");
    //    }

    //    try
    //    {
    //        //SceneManager.LoadScene(2);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError($"Failed to load scene: {ex.Message}");
    //    }
    //}

    //public void Level3()
    //{
    //    if (SoundManager.instance != null && clickSfx != null)
    //    {
    //        SoundManager.instance.PlaySound(clickSfx);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("SoundManager instance or clickSfx is null.");
    //    }

    //    try
    //    {
    //        //SceneManager.LoadScene(3);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError($"Failed to load scene: {ex.Message}");
    //    }
    //}

    //public void Level4()
    //{
    //    if (SoundManager.instance != null && clickSfx != null)
    //    {
    //        SoundManager.instance.PlaySound(clickSfx);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("SoundManager instance or clickSfx is null.");
    //    }

    //    try
    //    {
    //        //SceneManager.LoadScene(4);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError($"Failed to load scene: {ex.Message}");
    //    }
    //}

    //public void Demo()
    //{
    //    if (SoundManager.instance != null && clickSfx != null)
    //    {
    //        SoundManager.instance.PlaySound(clickSfx);

    //    }
    //    else
    //    {
    //        Debug.LogWarning("SoundManager instance or clickSfx is null.");
    //    }
    //    try
    //    {
    //        SceneManager.LoadScene(6);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError($"Failed to load scene: {ex.Message}");
    //    }
    //}
}
