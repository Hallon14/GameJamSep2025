using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class robert : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    int nextSpriteIndex;
    public List<Sprite> robertFaces = new List<Sprite>();

    public GameObject robertUI;
    public TextMeshProUGUI textRobert;
    private int robertSpeechIndex = 0;

    List<String> robertSpeech = new List<String>() {
        "Looks like you've lost your way on this ¨epic¨ quest of yours",
        "I've prepared a portal so that you may be on your way",
        "However it needs the power of 50 friends to activate",
        "So go on! 'Befriend' 50 of these lesser creatures, and yes, I obivously mean murder them. Slaughter them all!",
        "As you bring them back to life I'll consider them to be your 'friends'",
        "Robert. Out."
     };

    #region Basic Robert
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

    void OnEnable()
    {
        //Roberts speech during level 1
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            InputHandler.onInteract += level1Speech;
        }
    }
    #endregion

    #region Level 1 Robert
    public void level1Speech()
    {
        if (robertSpeechIndex == 6)
        {
            robertUI.SetActive(false);
            gameObject.SetActive(false);
        }
        if (robertSpeechIndex < 6)
        {
        textRobert.text = robertSpeech[robertSpeechIndex];  
        }

        robertSpeechIndex++;
    }
    
    #endregion
}
