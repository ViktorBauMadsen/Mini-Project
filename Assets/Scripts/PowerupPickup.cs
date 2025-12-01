using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    [Header("Visuals")]
    public float rotationSpeed = 45f;

    [Header("Boost Settings")]
    public float boostDuration = 20f;   // How long the boost lasts

    private void Update()
    {
        // Rotate the pickup for visual effect
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only the player can pick this up
        if (!other.CompareTag("Player"))
            return;

        // Try to get PlayerShooting from the player
        PlayerShooting shooting = other.GetComponentInParent<PlayerShooting>();

        if (shooting != null)
        {
            shooting.ActivateBoost(boostDuration);
        }

        // Destroy the pickup
        Destroy(gameObject);
    }
}
