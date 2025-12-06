using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Healing Settings")]
    // How much health this pickup restores
    public int healAmount = 1;

    [Header("Rotation Settings")]
    // Visual rotation speed
    public float rotationSpeed = 50f;

    [Header("Audio (Optional)")]
    // Sound played on pickup
    public AudioClip healSound;

    [Header("Particles (Optional)")]
    // Particle effect spawned on pickup
    public GameObject pickupEffect;

    private void Update()
    {
        // Rotate on Y axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                // Heal the player
                player.Heal(healAmount);

                // Play optional sound
                if (healSound != null)
                    AudioSource.PlayClipAtPoint(healSound, transform.position);

                // Spawn optional particle effect
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Remove the pickup
            Destroy(gameObject);
        }
    }
}
