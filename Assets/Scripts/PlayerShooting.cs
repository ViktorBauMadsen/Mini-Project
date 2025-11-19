using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Camera playerCamera;     // <-- add this!
    public float projectileSpeed = 20f;

    public Vector3 rotationOffset;  // Allow fixing prefab orientation

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // rotation the bullet should have (where the player is looking)
        Quaternion finalRot = playerCamera.transform.rotation * Quaternion.Euler(rotationOffset);

        // spawn bullet facing camera direction
        GameObject bullet = Instantiate(projectilePrefab, shootPoint.position, finalRot);

        // give it forward velocity in its own forward direction
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = bullet.transform.forward * projectileSpeed;
        }
    }
}
