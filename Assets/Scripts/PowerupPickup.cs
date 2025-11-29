using UnityEngine;
using System.Collections;

public class PowerupPickup : MonoBehaviour
{
    public float rotationSpeed = 45f;

    public float damageMultiplier = 2f;
    public float fireRateMultiplier = 0.5f;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerShooting shooting = other.GetComponentInParent<PlayerShooting>();

        if (shooting != null)
        {
            shooting.StartCoroutine(ApplyBoost(shooting));
        }

        Destroy(gameObject);
    }

    private IEnumerator ApplyBoost(PlayerShooting shooting)
    {
        Debug.Log("BOOST START");

        // Enable UI
        if (shooting.boostCanvas != null)
            shooting.boostCanvas.SetActive(true);

        float originalFireRate = shooting.fireRate;
        int originalDamage = Projectile.defaultDamage;

        shooting.fireRate = originalFireRate * fireRateMultiplier;
        Projectile.defaultDamage = Mathf.RoundToInt(originalDamage * damageMultiplier);

        Debug.Log("WAITING...");
        yield return new WaitForSeconds(20f);

        Debug.Log("BOOST END");

        // Reset stats
        shooting.fireRate = originalFireRate;
        Projectile.defaultDamage = originalDamage;

        // Disable UI
        if (shooting.boostCanvas != null)
            shooting.boostCanvas.SetActive(false);
    }
}
