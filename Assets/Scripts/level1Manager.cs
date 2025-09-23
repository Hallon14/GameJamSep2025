using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;


public class level1Manager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject portal;

    //Robert variables
    public GameObject robertUI;
    public GameObject robert;
    public TextMeshProUGUI textRobert;
    private int robertSpeechIndex = 0;

    public GameObject healthBar;

    List<String> robertSpeech = new List<String>() {
        "Looks like you've lost your way on this ¨epic¨ quest of yours",
        "I've prepared a portal so that you may be on your way",
        "However it needs the power of 50 friends to activate",
        "So go on! 'Befriend' 50 of these lesser creatures, and yes, I obivously mean murder them. Slaughter them all!",
        "As you bring them back to life I'll consider them to be your 'friends'",
        "Robert. Out."
    };

    void Awake()
    {
        InputHandler.onInteract += level1Speech;
    }

    public void setPortalActive()
    {
        portal.SetActive(true);
    }
    
    #region Level 1 Robert
    public void level1Speech()
    {
        if (robertSpeechIndex == 6)
        {
            robertUI.SetActive(false);
            robert.SetActive(false);
            healthBar.SetActive(true);
            InputHandler.onInteract -= level1Speech;
        }
        if (robertSpeechIndex < 6)
        {
            textRobert.text = robertSpeech[robertSpeechIndex];
        }

        robertSpeechIndex++;
    }
    
    #endregion
}
