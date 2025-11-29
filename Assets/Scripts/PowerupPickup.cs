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

        Destroy(gameObject);
    }

    private IEnumerator ApplyBoost(PlayerShooting shooting)
    {
        Debug.Log("BOOST START");

        float originalFireRate = shooting.fireRate;
        int originalDamage = Projectile.defaultDamage;

        shooting.fireRate = originalFireRate * fireRateMultiplier;
        Projectile.defaultDamage = Mathf.RoundToInt(originalDamage * damageMultiplier);

        Debug.Log("WAITING...");

        yield return new WaitForSeconds(boostDuration);

        Debug.Log("BOOST END");

        shooting.fireRate = originalFireRate;
        Projectile.defaultDamage = originalDamage;
    }

}
