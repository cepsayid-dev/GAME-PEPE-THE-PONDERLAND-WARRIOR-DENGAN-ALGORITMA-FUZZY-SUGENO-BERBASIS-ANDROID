using UnityEngine;

public class Click : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip clickSfx;

    public void OnClick() 
    {
        SoundManager.instance.PlaySound(clickSfx);
    }
}
