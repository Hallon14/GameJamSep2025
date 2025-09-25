using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth;
    [Header("Death Sequence")] public GameObject deathSequencePrefab; // optional death animation prefab
    private bool isDead = false;

    //send message when the player drops to 0 HP
    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath onPlayerDeath;

    // send message whenever the Player's HP changes.
    public delegate void OnPlayerHPChanged(float newHP);
    public static event OnPlayerHPChanged onPlayerHPChanged;
    public HitFlash hitFlash;

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

        hitFlash = GetComponent<HitFlash>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
            hitFlash.HitEffect();
            GetComponent<SoundPlayer>().PlayTakeDamageSound();
        }


    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //GetComponent<SoundPlayer>().PlayTakeDamageSound();
        onPlayerHPChanged?.Invoke(currentHealth / maxHealth); // ensure floating point ratio
        if (currentHealth <= 0 && !isDead)
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
        if (isDead) return;
        isDead = true;
        // Spawn death sequence before hiding sprite
        if (deathSequencePrefab != null)
        {
            Instantiate(deathSequencePrefab, transform.position, transform.rotation);
        }
        onPlayerDeath?.Invoke();
        GetComponent<SpriteRenderer>().enabled = false;

        if (GameManager.Instance)
        {
            GameManager.Instance.gameOver();
        }

    }
}
