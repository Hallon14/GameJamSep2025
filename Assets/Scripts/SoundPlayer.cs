using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip[] attackClips;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClips[Random.Range(0, attackClips.Length)]);
    }


}
