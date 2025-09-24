using System.Collections.Generic;
using UnityEngine;

public class fireAnimator : MonoBehaviour
{

    private int nextSpriteIndex;
    public List<Sprite> fireSprites = new List<Sprite>();
    private SpriteRenderer spriteRenderer;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(animateFire), 1, 1);
    }

    private void animateFire()
    {
        spriteRenderer.sprite = fireSprites[nextSpriteIndex];
        nextSpriteIndex++;
        if (nextSpriteIndex == 7)
        {
            nextSpriteIndex = 0;
        }
        else
        {
            nextSpriteIndex++;
        }
    }
}
