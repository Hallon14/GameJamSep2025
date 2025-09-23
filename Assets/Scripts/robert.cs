using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class robert : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    int nextSpriteIndex;
    public List<Sprite> robertFaces = new List<Sprite>();

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextSpriteIndex = 0;

        
        InvokeRepeating(nameof(swapSprite), .5f, .5f);
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
