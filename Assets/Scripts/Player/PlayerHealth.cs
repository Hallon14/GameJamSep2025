using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    //send message when the player drops to 0 HP
    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath onPlayerDeath;

    // send message whenever the Player's HP changes.
    public delegate void OnPlayerHPChanged(float newHP);
    public static event OnPlayerHPChanged onPlayerHPChanged;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
        }


    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        onPlayerHPChanged?.Invoke(currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        onPlayerDeath?.Invoke();
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
