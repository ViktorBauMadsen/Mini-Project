using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 3f;
    public static int defaultDamage = 1;   // <--- ADD THIS
    public int damage = 1;

    private void Start()
    {
        damage = defaultDamage;            // <--- ADD THIS
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
