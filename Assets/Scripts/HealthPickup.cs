using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Healing Settings")]
    public int healAmount = 1;

    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;

    [Header("Audio (Optional)")]
    public AudioClip healSound;

    [Header("Particles (Optional)")]
    public GameObject pickupEffect;

    private void Update()
    {
        // Rotate on Y axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.Heal(healAmount);

                if (healSound != null)
                    AudioSource.PlayClipAtPoint(healSound, transform.position);

                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
