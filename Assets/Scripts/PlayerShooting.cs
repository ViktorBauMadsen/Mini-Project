using UnityEngine;
using System.Collections;

// Handles player shooting, recoil and temporary fire-rate boosts
public class PlayerShooting : MonoBehaviour
{
    [Header("Boost UI")]
    // UI shown while boost active
    public GameObject boostCanvas;

    [Header("Shooting")]
    // Bullet prefab to spawn
    public GameObject projectilePrefab;
    // Where projectiles spawn
    public Transform shootPoint;
    // Player camera for aim direction
    public Camera playerCamera;
    // Speed applied to spawned projectiles
    public float projectileSpeed = 20f;

    // Rotation offset applied to camera rotation for projectile orientation
    public Vector3 rotationOffset;

    [Header("Fire Rate")]
    // Current fire rate (seconds between shots)
    public float fireRate = 0.1f; // actively used during gameplay
    private float nextShootTime = 0f;

    [Header("Recoil Settings")]
    // Transform of the gun used for recoil movement
    public Transform gunTransform;
    // How far the gun recoils back
    public float recoilDistance = 0.1f;
    // How fast the gun moves back
    public float recoilSpeed = 20f;
    // How fast the gun returns
    public float returnSpeed = 10f;

    private Vector3 gunOriginalLocalPos;
    private bool isRecoiling = false;

    [Header("Gun Audio")]
    // Audio source used to play shoot sounds
    public AudioSource audioSource;
    public AudioClip shootSound;
    public float shootVolume = 1f;

    // ---------------- BOOST SETTINGS ----------------
    [Header("Boost Settings (Non-Stacking)")]
    // Base fire rate before boost
    public float normalFireRate = 0.1f;      // default fire rate
    // Fire rate while boosted
    public float boostedFireRate = 0.03f;    // boosted fire rate

    // Bullet damage values (used via Projectile.damage)
    public int normalDamage = 1;             // default bullet damage
    public int boostedDamage = 3;            // boosted bullet damage

    // Prevent multiple boosts stacking
    private bool boostActive = false;        // prevents stacking
    // -------------------------------------------------

    private void Start()
    {
        // Cache original gun local position
        if (gunTransform != null)
            gunOriginalLocalPos = gunTransform.localPosition;

        // Ensure default stats are applied
        fireRate = normalFireRate;
        Projectile.damage = normalDamage;
    }

    private void Update()
    {
        // Hold left mouse to shoot, respecting fire rate
        if (Input.GetMouseButton(0) && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Build rotation from camera + offset
        Quaternion finalRot = Quaternion.Euler(
            playerCamera.transform.eulerAngles.x + rotationOffset.x,
            playerCamera.transform.eulerAngles.y + rotationOffset.y,
            playerCamera.transform.eulerAngles.z + rotationOffset.z
        );

        // Spawn the projectile
        GameObject bullet = Instantiate(projectilePrefab, shootPoint.position, finalRot);

        // Give it forward velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = bullet.transform.forward * projectileSpeed;

        // Start recoil coroutine if not already running
        if (!isRecoiling && gunTransform != null)
            StartCoroutine(RecoilRoutine());

        // Play shooting sound (can overlap)
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound, shootVolume);
    }

    IEnumerator RecoilRoutine()
    {
        isRecoiling = true;

        Vector3 targetPos = gunOriginalLocalPos + Vector3.back * recoilDistance;

        // Move backwards toward target recoil position
        while (Vector3.Distance(gunTransform.localPosition, targetPos) > 0.001f)
        {
            gunTransform.localPosition = Vector3.Lerp(
                gunTransform.localPosition,
                targetPos,
                Time.deltaTime * recoilSpeed
            );
            yield return null;
        }

        // Return to original position smoothly
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
        // Ignore if already boosted
        if (boostActive)
            return;

        boostActive = true;

        // Apply boosted fire rate only
        fireRate = boostedFireRate;

        // Show boost UI if assigned
        if (boostCanvas != null)
            boostCanvas.SetActive(true);

        StartCoroutine(BoostTimer(duration));
    }

    private IEnumerator BoostTimer(float duration)
    {
        // Wait for boost duration
        yield return new WaitForSeconds(duration);

        // Revert to normal stats
        boostActive = false;
        fireRate = normalFireRate;

        // Hide boost UI
        if (boostCanvas != null)
            boostCanvas.SetActive(false);
    }

    // ------------------------------------------------------

}
