using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [Header("Boost UI")]
    public GameObject boostCanvas;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Camera playerCamera;
    public float projectileSpeed = 20f;

    public Vector3 rotationOffset;

    [Header("Fire Rate")]
    public float fireRate = 0.1f; // actively used during gameplay
    private float nextShootTime = 0f;

    [Header("Recoil Settings")]
    public Transform gunTransform;
    public float recoilDistance = 0.1f;
    public float recoilSpeed = 20f;
    public float returnSpeed = 10f;

    private Vector3 gunOriginalLocalPos;
    private bool isRecoiling = false;

    [Header("Gun Audio")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    public float shootVolume = 1f;

    // ---------------- BOOST SETTINGS ----------------
    [Header("Boost Settings (Non-Stacking)")]
    public float normalFireRate = 0.1f;      // default fire rate
    public float boostedFireRate = 0.03f;    // boosted fire rate

    public int normalDamage = 1;             // default bullet damage
    public int boostedDamage = 3;            // boosted bullet damage

    private bool boostActive = false;        // prevents stacking
    // -------------------------------------------------

    private void Start()
    {
        if (gunTransform != null)
            gunOriginalLocalPos = gunTransform.localPosition;

        // Ensure initial values are correct
        fireRate = normalFireRate;
        Projectile.damage = normalDamage;
    }

    private void Update()
    {
        // HOLD LEFT CLICK + respects fireRate
        if (Input.GetMouseButton(0) && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Apply camera rotation + offset
        Quaternion finalRot = Quaternion.Euler(
            playerCamera.transform.eulerAngles.x + rotationOffset.x,
            playerCamera.transform.eulerAngles.y + rotationOffset.y,
            playerCamera.transform.eulerAngles.z + rotationOffset.z
        );

        // Spawn projectile
        GameObject bullet = Instantiate(projectilePrefab, shootPoint.position, finalRot);

        // Add forward velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = bullet.transform.forward * projectileSpeed;

        // Trigger recoil
        if (!isRecoiling && gunTransform != null)
            StartCoroutine(RecoilRoutine());

        // Play gun sound (overlapping allowed)
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound, shootVolume);
    }

    IEnumerator RecoilRoutine()
    {
        isRecoiling = true;

        Vector3 targetPos = gunOriginalLocalPos + Vector3.back * recoilDistance;

        // Move backwards
        while (Vector3.Distance(gunTransform.localPosition, targetPos) > 0.001f)
        {
            gunTransform.localPosition = Vector3.Lerp(
                gunTransform.localPosition,
                targetPos,
                Time.deltaTime * recoilSpeed
            );
            yield return null;
        }

        // Return to original position
        while (Vector3.Distance(gunTransform.localPosition, gunOriginalLocalPos) > 0.001f)
        {
            gunTransform.localPosition = Vector3.Lerp(
                gunTransform.localPosition,
                gunOriginalLocalPos,
                Time.deltaTime * returnSpeed
            );
            yield return null;
        }

        gunTransform.localPosition = gunOriginalLocalPos;
        isRecoiling = false;
    }

    // ---------------- BOOST FUNCTIONALITY ----------------

    public void ActivateBoost(float duration)
    {
        // Prevent stacking
        if (boostActive)
            return;

        boostActive = true;

        // Apply boosted stats
        fireRate = boostedFireRate;
        Projectile.damage = boostedDamage;

        // Enable UI
        if (boostCanvas != null)
            boostCanvas.SetActive(true);

        StartCoroutine(BoostTimer(duration));
    }

    private IEnumerator BoostTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Reset to original stats
        boostActive = false;
        fireRate = normalFireRate;
        Projectile.damage = normalDamage;

        // Disable UI
        if (boostCanvas != null)
            boostCanvas.SetActive(false);
    }

    // ------------------------------------------------------
}
