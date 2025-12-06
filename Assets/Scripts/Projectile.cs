using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Time before projectile self-destructs
    public float lifetime = 3f;

    // GLOBAL DAMAGE VALUE that powerup will modify
    // Use this to change the default damage for new projectiles
    public static int defaultDamage = 1;

    // Local per-projectile value (set from default on Start)
    public static int damage = 1;

    private void Start()
    {
        // Initialize this projectile's damage from the global default
        damage = defaultDamage;

        // Destroy after lifetime seconds to avoid lingering bullets
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If we hit an enemy, apply damage
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.collider.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        // Destroy projectile on any collision
        Destroy(gameObject);
    }
}
