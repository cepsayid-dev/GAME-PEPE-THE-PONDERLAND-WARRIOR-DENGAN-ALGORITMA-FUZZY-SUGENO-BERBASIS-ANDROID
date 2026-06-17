using UnityEngine;

public class Fuzzy : MonoBehaviour
{
    [Header("Fuzzy Settings")]
    public Transform player; // Referensi ke pemain
    public float updateInterval = 0.5f; // Interval pembaruan logika fuzzy

    // Variabel input
    protected float distanceToPlayer;

    protected virtual void Start()
    {
        InvokeRepeating("UpdateFuzzyLogic", 0f, updateInterval);
    }

    protected virtual void UpdateFuzzyLogic()
    {
        // Hitung jarak ke pemain
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }

    // Fungsi keanggotaan untuk jarak (Close/Far)
    protected float MembershipClose(float distance)
    {
        if (distance <= 5) return 1f;
        if (distance >= 10) return 0f;
        return (10 - distance) / 5f; // Linear turun dari 5-10
    }

    protected float MembershipFar(float distance)
    {
        return 1f - MembershipClose(distance);
    }

    // Abstract method untuk defuzzifikasi (diimplementasikan di subclass)
    protected Vector2 Defuzzify()
        {
        // Gerakkan monster horizontal
        Vector2 direction = (player.position - transform.position).normalized;
        return direction;
        
    }
}
