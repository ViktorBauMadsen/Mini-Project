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
        // Try to get PlayerShooting component from the player
        PlayerShooting shooting = other.GetComponentInParent<PlayerShooting>();

        if (shooting != null)
        {
            shooting.ActivateBoost(boostDuration);   // Use NEW boost system
        }

        // Destroy the pickup after collection
        Destroy(gameObject);
    }
}
