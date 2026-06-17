using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioClip clickSfx;

    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject Guide;
    [SerializeField] private GameObject ExitPopUp;

    private void Awake()
    {
        if (settings && Guide != null)
        {
            settings.SetActive(false);
            Guide.SetActive(false);
            ExitPopUp.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Settings GameObject is not assigned.");
        }
    }

    //Settings
    public void Settings()
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        if (settings != null)
        {
            settings.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Settings GameObject is not assigned.");
        }
    }

    //Back on Main Menu screen
    public void MainBack()
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        if (settings != null)
        {
            settings.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Settings GameObject is not assigned.");
        }
    }

    //Quit
    public void Quit()
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        Application.Quit();
    }

    public void BackE() 
    {
        ExitPopUp.SetActive(false);
    }

    //Guide
    public void HowTo()
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        if (Guide != null)
        {
            Guide.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Settings GameObject is not assigned.");
        }
    }

    public void HowToBack() 
    {
        Guide.SetActive(false);
    }

    //PopUUpExit
    public void ExitPO()
    {
        if (SoundManager.instance != null && clickSfx != null)
        {
            SoundManager.instance.PlaySound(clickSfx);
        }
        else
        {
            Debug.LogWarning("SoundManager instance or clickSfx is null.");
        }

        if (ExitPopUp != null)
        {
            ExitPopUp.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Settings GameObject is not assigned.");
        }
    }

    //Start or Level Selection Menu
    public void Play()
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
            SceneManager.LoadScene(5);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("Button clicked!");
    }
}
