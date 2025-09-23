using UnityEngine;
using System.Collections.Generic;


public class Anton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    int nextSpriteIndex;
    public List<Sprite> antonFaces = new List<Sprite>();
    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextSpriteIndex = 0;

        //Swap sprites every .15 seconds.
        InvokeRepeating(nameof(swapSprite), 0.25f, 0.25f);
    }

    private void swapSprite()
    {
        spriteRenderer.sprite = antonFaces[nextSpriteIndex];
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