using UnityEngine;
using System.Collections;

public class PowerupPickup : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 45f;

    [Header("Powerup Stats")]
    public float boostDuration = 8f;
    public float damageMultiplier = 2f;
    public float fireRateMultiplier = 0.5f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerShooting shooting = other.GetComponent<PlayerShooting>();
        if (shooting != null)
        {
            StartCoroutine(ApplyBoost(shooting));
        }

        Destroy(gameObject); // consume pickup
    }

    private IEnumerator ApplyBoost(PlayerShooting shooting)
    {
        // Save original values
        int originalDamage = Projectile.defaultDamage;
        float originalFireRate = shooting.fireRate;

        // Apply boost
        Projectile.defaultDamage = Mathf.RoundToInt(originalDamage * damageMultiplier);
        shooting.fireRate *= fireRateMultiplier;

        yield return new WaitForSeconds(boostDuration);

        // Reset values
        Projectile.defaultDamage = originalDamage;
        shooting.fireRate = originalFireRate;
    }
}
