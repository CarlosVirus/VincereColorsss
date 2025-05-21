using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public enum Team { Player, Enemy }
    public Team team;

    public int maxHealth = 100;
    private int currentHealth;

    public GameObject healthBarCanvas;
    public Image healthBarFill;

    public bool isTownHall = false;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBarCanvas.SetActive(false);
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < maxHealth)
        {
            healthBarCanvas.SetActive(true);
        }

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    protected virtual void Die()
    {
        if (isTownHall)
        {
            Debug.Log("🏛️ Ayuntamiento destruido. Has perdido o ganado la partida.");
        }

        Destroy(gameObject);
    }
}
