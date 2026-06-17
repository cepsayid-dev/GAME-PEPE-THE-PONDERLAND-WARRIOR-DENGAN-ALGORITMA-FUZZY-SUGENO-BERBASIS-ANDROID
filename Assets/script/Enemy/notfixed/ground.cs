using UnityEngine;

public class ground : NPCAIController
{
    //[Header("Ground Settings")]
    //public float moveSpeed = 2f;
    //public float attackRange =1.5f;

    //// Variabel tambahan untuk monster darat
    //private float horizontalMovement;

    //protected override void UpdateFuzzyLogic()
    //{
    //    base.UpdateFuzzyLogic();

    //    // Rule Evaluation (Contoh)
    //    float close = MembershipClose(distanceToPlayer);
    //    float far = MembershipFar(distanceToPlayer);

    //    // Aturan Fuzzy Sugeno untuk monster darat:
    //    // Rule 1: IF distance Close THEN horizontalMovement = 0 (berhenti)
    //    // Rule 2: IF distance Far THEN horizontalMovement = 1 (mendekat)
    //    float rule1Strength = close;
    //    float rule2Strength = far;

    //    // Hitung output defuzzifikasi
    //    horizontalMovement = (rule1Strength * 0 + rule2Strength * 1) / (rule1Strength + rule2Strength);
    //}

    //protected override void Defuzzify()
    //{
    //    // Gerakkan monster horizontal
    //    Vector2 direction = (player.position - transform.position).normalized;
    //    transform.Translate(direction * horizontalMovement * moveSpeed * Time.deltaTime);

    //    // Serang jika dalam jarak
    //    if (distanceToPlayer <= attackRange)
    //    {
    //        Debug.Log("Slime menyerang!");
    //    }
    //}

    //private void Update()
    //{
    //    Vector2 direction = Defuzzify();
    //    transform.Translate(direction * horizontalMovement * moveSpeed * Time.deltaTime);

    //    // Serang jika dalam jarak
    //    if (distanceToPlayer <= attackRange)
    //    {
    //        Debug.Log("Slime menyerang!");
    //    }
    //}
}
