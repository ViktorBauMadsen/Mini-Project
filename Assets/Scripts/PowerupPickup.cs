using UnityEngine;

// Simple pickup that gives the player a temporary boost
public class PowerupPickup : MonoBehaviour
{
    [Header("Visuals")]
    // Rotation speed for the floating pickup
    public float rotationSpeed = 45f;

    [Header("Boost Settings")]
    // How long the boost lasts (seconds)
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

        // Try to get PlayerShooting from the player and activate boost
        PlayerShooting shooting = other.GetComponentInParent<PlayerShooting>();

        if (shooting != null)
        {
            shooting.ActivateBoost(boostDuration);
        }

        // Remove the pickup after use
        Destroy(gameObject);
    }
}
