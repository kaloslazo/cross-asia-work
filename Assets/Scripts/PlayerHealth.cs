using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage: " + amount + ". Current health: " + currentHealth);

        AudioManager.instance.Play("male-damage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log("Player healed: " + amount + ". Current health: " + currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        AudioManager.instance.Play("male-died");
        DisableAllZombies();
        FindObjectOfType<DieMenu>().ShowDieMenu();
    }

    private void DisableAllZombies()
    {
        ZombieBehavior[] zombies = FindObjectsOfType<ZombieBehavior>();
        foreach (ZombieBehavior zombie in zombies)
        {
            zombie.gameObject.SetActive(false);  // Desactiva el objeto de cada zombie
        }
    }
}
