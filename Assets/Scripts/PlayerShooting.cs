using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Camera playerCamera;
    public float projectileSpeed = 20f;

    public Vector3 rotationOffset;

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

    private void Start()
    {
        if (gunTransform != null)
            gunOriginalLocalPos = gunTransform.localPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Apply camera rotation + your rotation offset (world-space)
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
        {
            rb.linearVelocity = bullet.transform.forward * projectileSpeed;
        }

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
}
