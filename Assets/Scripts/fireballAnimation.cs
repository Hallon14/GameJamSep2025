using System.Collections.Generic;
using UnityEngine;

public class fireballAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int nextSpriteIndex = 0;
    [SerializeField]
    private List<Sprite> fireBall = new List<Sprite>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(swapSprite), .15f, .15f);
    }
    private void swapSprite()
    {
        spriteRenderer.sprite = fireBall[nextSpriteIndex];
        if (nextSpriteIndex == 1)
        {
            nextSpriteIndex = 0;
        }
        else
        {
            nextSpriteIndex++;
        }
    }
}
