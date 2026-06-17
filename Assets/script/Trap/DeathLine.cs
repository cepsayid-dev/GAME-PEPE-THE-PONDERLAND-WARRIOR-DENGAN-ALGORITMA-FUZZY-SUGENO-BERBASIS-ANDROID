using UnityEngine;

public class DeathLine : MonoBehaviour
{

    [SerializeField] private int trapDamage = 100;
    [SerializeField] private float damageCooldown = 1.0f; // Time in seconds between each damage

    private float lastDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null && damageable.IsAlive)
        {
            // Check if enough time has passed since the last damage application
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                damageable.TakeDamage(trapDamage);
                lastDamageTime = Time.time;
                Debug.Log("Trap inflicted repeated damage on the player.");
            }
        }
    }

}
