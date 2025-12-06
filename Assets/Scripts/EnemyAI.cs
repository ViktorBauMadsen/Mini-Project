using UnityEngine;
using UnityEngine.AI;

// Simple NavMesh-based enemy AI that chases and attacks the player
public class EnemyAI : MonoBehaviour
{
    // Damage dealt to the player
    public int damage = 1;
    // Distance within which the enemy will attack
    public float attackRange = 1.5f;
    // Time between attack attempts
    public float attackCooldown = 1f;

    // Tracks when the next attack is allowed
    private float nextAttackTime;
    // Animator for attack animations
    private Animator anim;
    // NavMeshAgent for movement
    public NavMeshAgent agent;
    // Reference to player transform
    private Transform player;

    void Awake()
    {
        // Cache player, animator and agent
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // If far from player, chase
        if (dist > attackRange)
        {
            anim.SetBool("isAttacking", false);
            agent.isStopped = false;
            agent.SetDestination(player.position);
            return;
        }

        // Within attack range: stop moving and try to attack
        agent.isStopped = true;

        if (Time.time >= nextAttackTime)
        {
            anim.SetBool("isAttacking", true);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // Called by animation event to apply damage
    public void DealDamage()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        anim.SetBool("isAttacking", false);
    }
}
