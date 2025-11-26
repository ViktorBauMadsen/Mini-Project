using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public int damage = 1;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    private float nextAttackTime;
    private Animator anim;
    public NavMeshAgent agent;
    private Transform player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            anim.SetBool("isAttacking", false);
            agent.isStopped = false;
            agent.SetDestination(player.position);
            return;
        }

        // Stop moving to attack
        agent.isStopped = true;

        // Attack EXACTLY when cooldown is ready
        if (Time.time >= nextAttackTime)
        {
            anim.SetBool("isAttacking", true);   // Play animation
            nextAttackTime = Time.time + attackCooldown;  // Start cooldown
        }
    }

    // Animation Event
    public void DealDamage()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        // Reset attack animation immediately for next attack
        anim.SetBool("isAttacking", false);
    }
}
