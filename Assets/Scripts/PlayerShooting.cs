using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Camera playerCamera;
    public float projectileSpeed = 20f;

    public Vector3 rotationOffset;  // Allows fixing prefab orientation (e.g., X = 90)

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

        // Add forward velocity (using its own forward direction)
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = bullet.transform.forward * projectileSpeed;
        }
    }
}
