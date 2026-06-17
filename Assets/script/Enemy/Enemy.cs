using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyDmg = 20;
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
                damageable.TakeDamage(enemyDmg);
                lastDamageTime = Time.time;
                Debug.Log("Enemy inflicted repeated damage on the player.");
            }
        }
    }
}