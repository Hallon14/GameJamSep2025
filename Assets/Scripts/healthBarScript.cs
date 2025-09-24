using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class healthBarScript : MonoBehaviour
{

    public List<Sprite> sprites = new List<Sprite>();
    public GameObject fountain;
    private int spriteIndex = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        InvokeRepeating(nameof(swapSprite), 0, 1.5F);
    }


    void swapSprite()
    {
        fountain.GetComponent<Image>().sprite = sprites[spriteIndex];
        if (spriteIndex == 1)
        {
            spriteIndex = 0;
        }
        else
        {
            spriteIndex++;
        }
    }
}
