using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public Image healthBarFill;

    float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateBar();
    }

    public void TakeDamage(float dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        UpdateBar();
        if (currentHealth == 0) Destroy(gameObject);
    }

    void UpdateBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = currentHealth / maxHealth;
    }
}
