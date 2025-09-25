using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth;

    //send message when the player drops to 0 HP
    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath onPlayerDeath;

    // send message whenever the Player's HP changes.
    public delegate void OnPlayerHPChanged(float newHP);
    public static event OnPlayerHPChanged onPlayerHPChanged;

    void Start()
    {
        if (GameManager.Instance)
        {
            currentHealth = GameManager.Instance.playerHealth;
        }
        else
        {
            currentHealth = maxHealth;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
        }


    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        onPlayerHPChanged?.Invoke(currentHealth / maxHealth); // ensure floating point ratio
        if (currentHealth <= 0)
        {
            KillPlayer();
        }
        if (GameManager.Instance)
        {
            GameManager.Instance.updatePlayerHealth(currentHealth);
        }

    }

    public void KillPlayer()
    {
        onPlayerDeath?.Invoke();
        GetComponent<SpriteRenderer>().enabled = false;
        GameManager.Instance.gameOver();
    }
}
