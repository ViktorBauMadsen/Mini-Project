using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private PlayerHealth playerHealth;

    [Header("Attack Settings")]
    public int damage = 1;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;

    private float lastAttackTime = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerHealth = p.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (player == null || agent == null)
            return;

        // ---- MOVEMENT (your original logic) ----
        agent.SetDestination(player.position);

        // ---- ATTACK LOGIC ----
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy attacked player!");
            }
        }
    }
}
