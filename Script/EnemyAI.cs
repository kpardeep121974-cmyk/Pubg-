using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("AI Settings")]
    public float visualRange = 15f;
    public float attackRange = 8f;
    public float patrolRadius = 20f;

    [Header("Combat Settings")]
    public float fireRate = 1f;
    private float nextTimeToFire = 0f;

    // States
    private enum AIState { Patrolling, Chasing, Attacking }
    private AIState currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // "Player" tag wale object ko dhoondna
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        currentState = AIState.Patrolling;
        MoveToRandomPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // State Machine Logic
        switch (currentState)
        {
            case AIState.Patrolling:
                PatrolLogic(distanceToPlayer);
                break;
            case AIState.Chasing:
                ChaseLogic(distanceToPlayer);
                break;
            case AIState.Attacking:
                AttackLogic(distanceToPlayer);
                break;
        }
    }

    // 1. Patrolling Logic (Zameen par bina wajah ghoomna)
    void PatrolLogic(float distanceToPlayer)
    {
        if (distanceToPlayer <= visualRange)
        {
            currentState = AIState.Chasing;
            return;
        }

        // Agar bot apni jagah par pahunch gaya hai, toh naya point dhoondein
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToRandomPatrolPoint();
        }
    }

    void MoveToRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    // 2. Chasing Logic (Player ke peeche daudna)
    void ChaseLogic(float distanceToPlayer)
    {
        if (distanceToPlayer > visualRange)
        {
            currentState = AIState.Patrolling;
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            currentState = AIState.Attacking;
            return;
        }

        agent.SetDestination(player.position);
    }

    // 3. Attacking Logic (Player par goli chalana)
    void AttackLogic(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange)
        {
            currentState = AIState.Chasing;
            return;
        }

        // Player ki taraf dekhna aur rukna
        agent.SetDestination(transform.position); 
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Shooting Interval (Goli chalane ka gap)
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("Bot is Shooting at Player!");
        // Yahan aap apni WeaponShooting script ka function call kar sakte hain
    }

    // Unity Editor mein ranges dekhne ke liye gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visualRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
