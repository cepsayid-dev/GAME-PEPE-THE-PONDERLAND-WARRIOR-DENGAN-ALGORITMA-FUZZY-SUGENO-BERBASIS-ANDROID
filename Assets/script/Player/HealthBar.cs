using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
     public Slider hB;
     public TMPro.TMP_Text healthbartxt;

    Damageable pDamagable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pDamagable = Object.FindFirstObjectByType<Damageable>();

        if (pDamagable != null)
        {
            hB.value = CalculateSliderPercentage(pDamagable.Health, pDamagable.MaxHealth);
            healthbartxt.text = pDamagable.Health + " / " + pDamagable.MaxHealth;
            pDamagable.healthChanged.AddListener(OnPlayerHealthChanged);
        }
        else
        {
            Debug.LogError("Damageable component not found in the scene.");
        }
    }

    private float CalculateSliderPercentage(float cHealth, float maxHealth)
    {
        return cHealth / maxHealth;
    }


    private void OnPlayerHealthChanged(int newhealth, int maxHealth)
    {
        hB.value = CalculateSliderPercentage(newhealth, maxHealth);
        healthbartxt.text = "Hp " + newhealth + " / " + maxHealth;
    }



}
