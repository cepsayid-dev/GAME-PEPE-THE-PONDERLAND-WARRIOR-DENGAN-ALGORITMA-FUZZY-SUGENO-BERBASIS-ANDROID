using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int heal = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has a Damageable component
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null && damageable.IsAlive)
        {
            // Heal the player, ensuring health doesn't exceed max health
            int newHealth = Mathf.Min(damageable.Health + heal, damageable.MaxHealth);
            if (newHealth > damageable.Health) // Only heal if needed
            {
                damageable.Health = newHealth;
                // Optionally, you can add an effect or sound here

                // Destroy the pickup after use
                Destroy(gameObject);
            }
        }
    }
}
