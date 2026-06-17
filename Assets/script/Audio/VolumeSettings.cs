using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Sound Manager Reference")]
    public SoundManager soundManager;
    public GameObject bgmObject;

    [Header("Test SFX Clip")]
    public AudioClip testSfxClip;

    private AudioSource bgmSource;

    private const string BgmVolumeKey = "BGM_VOLUME";
    private const string SfxVolumeKey = "SFX_VOLUME";

    private float lastSfxSliderValue;

    void Start()
    {
        if (bgmObject != null)
        {
            bgmSource = bgmObject.GetComponent<AudioSource>();
        }

        // Load saved volume settings or set defaults
        float savedBgmVolume = PlayerPrefs.GetFloat(BgmVolumeKey, 0.5f);
        float savedSfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 0.5f);

        bgmSlider.value = savedBgmVolume;
        sfxSlider.value = savedSfxVolume;

        lastSfxSliderValue = savedSfxVolume;

        ApplyBgmVolume(savedBgmVolume);
        ApplySfxVolume(savedSfxVolume);

        // Add listeners to sliders
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);

        // Add PointerUp event listener to SFX slider
        EventTrigger trigger = sfxSlider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        entry.callback.AddListener((_) => OnSfxSliderReleased());
        trigger.triggers.Add(entry);
    }

    private void OnBgmSliderChanged(float value)
    {
        ApplyBgmVolume(value);
        PlayerPrefs.SetFloat(BgmVolumeKey, value);
    }

    private void OnSfxSliderChanged(float value)
    {
        ApplySfxVolume(value);
        PlayerPrefs.SetFloat(SfxVolumeKey, value);
    }

    public void OnSfxSliderReleased()
    {
        // Play test SFX to preview volume when slider is released
        if (soundManager != null && testSfxClip != null)
        {
            soundManager.PlaySound(testSfxClip);
        }
    }

    private void ApplyBgmVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    private void ApplySfxVolume(float volume)
    {
        if (soundManager != null)
        {
            soundManager.GetComponent<AudioSource>().volume = volume;
        }
    }

    private void OnApplicationQuit()
    {
        // Save settings on quit
        PlayerPrefs.Save();
    }
}
