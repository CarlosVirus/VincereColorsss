using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Team team;
    public GameObject selectionVisual;

    public int maxHealth = 2;
    private int currentHealth;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    public void MoveTo(Vector3 p) => agent.SetDestination(p);

    public void SetSelected(bool s)
    {
        if (selectionVisual != null) selectionVisual.SetActive(s);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
