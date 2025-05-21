using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float attackDamage = 25f;

    private float lastHitTime;
    private Unit self;

    void Awake()
    {
        self = GetComponent<Unit>();
    }

    void Update()
    {
        // Aquí deberías tener lógica para encontrar al objetivo a atacar (targetUnit),
        // pero como no la has incluido, asumiré que ya tienes el target preparado en alguna parte.
        // Simulación de target:
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider col in hits)
        {
            Unit targetUnit = col.GetComponent<Unit>();
            if (targetUnit != null && targetUnit.team != self.team)
            {
                if (Time.time - lastHitTime < attackCooldown)
                    return;

                Health health = targetUnit.GetComponent<Health>();
                if (health != null)
                {
                    // 🚨 El enemigo atacado pide ayuda a sus aliados
                    EnemyAI enemyAI = targetUnit.GetComponent<EnemyAI>();
                    if (enemyAI != null)
                    {
                        enemyAI.RequestHelp(transform.position);
                    }

                    health.TakeDamage(attackDamage);
                    lastHitTime = Time.time;
                    return;
                }
            }

            // Atacar construcciones enemigas
            Building building = col.GetComponent<Building>();
            if (building != null && building.gameObject.layer != gameObject.layer)
            {
                if (Time.time - lastHitTime < attackCooldown)
                    return;

                building.TakeDamage((int)attackDamage);
                lastHitTime = Time.time;
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
