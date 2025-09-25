using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip[] attackClips;
    public AudioClip[] takeDamageClips;
    public AudioClip[] deathClips;

    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {


    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClips[Random.Range(0, attackClips.Length)]);
    }

    public void PlayTakeDamageSound()
    {
        audioSource.PlayOneShot(takeDamageClips[Random.Range(0, takeDamageClips.Length)]);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathClips[Random.Range(0, deathClips.Length)]);
    }


}
