using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 3f;

    // GLOBAL DAMAGE VALUE that powerup will modify
    public static int defaultDamage = 1;

    // Local per-projectile value
    public int damage = 1;

    private void Start()
    {
        // Set projectile damage from the global damage
        damage = defaultDamage;

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.collider.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
