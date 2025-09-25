using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField]
    private float flashDuration = 0.05f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void HitEffect()
    {
        StartCoroutine(PlayHitEffect());
    }

    IEnumerator PlayHitEffect()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

}