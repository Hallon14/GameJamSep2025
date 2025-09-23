using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class robert : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    int nextSpriteIndex;
    public List<Sprite> robertFaces = new List<Sprite>();

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextSpriteIndex = 0;
        
        //Swap sprites every .15 seconds.
        InvokeRepeating(nameof(swapSprite), 0.25f, 0.25f);
    }

    private void swapSprite()
    {
        spriteRenderer.sprite = robertFaces[nextSpriteIndex];
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
