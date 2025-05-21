using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float detectionRange = 5f;

    private NavMeshAgent agent;
    private float timer;
    private Unit self;

    private GameObject currentTarget;
    private Vector3? helpPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        self = GetComponent<Unit>();
        timer = wanderTimer;
    }

    void Update()
    {
        if (helpPosition.HasValue)
        {
            agent.SetDestination(helpPosition.Value);

            if (Vector3.Distance(transform.position, helpPosition.Value) < 2f)
            {
                helpPosition = null;
            }
            else
            {
                DetectAndChaseEnemy(); // Sigue atacando si ve enemigos
            }
            return;
        }

        if (DetectAndChaseEnemy()) return;

        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    bool DetectAndChaseEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider hit in hits)
        {
            Unit targetUnit = hit.GetComponent<Unit>();
            if (targetUnit != null && targetUnit.team != self.team)
            {
                agent.SetDestination(targetUnit.transform.position);
                return true;
            }
        }

        return false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void RequestHelp(Vector3 attackerPosition)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 10f);

        foreach (Collider col in hits)
        {
            EnemyAI ally = col.GetComponent<EnemyAI>();
            if (ally != null && ally != this)
            {
                ally.RespondToHelp(attackerPosition);
            }
        }
    }

    public void RespondToHelp(Vector3 targetPos)
    {
        helpPosition = targetPos;
        agent.SetDestination(targetPos);
    }
}

