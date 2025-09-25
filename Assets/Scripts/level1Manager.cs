using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;


public class level1Manager : MonoBehaviour
{

    //Robert variables
    public GameObject robertUI;
    public GameObject robert;
    public TextMeshProUGUI textRobert;
    private int robertSpeechIndex = 0;

    public GameObject healthBar;
    public GameObject friendsBar;

    List<String> robertSpeech = new List<String>() {
        "Looks like you've lost your way on this ¨epic¨ quest of yours",
        "I've prepared a portal so that you may be on your way",
        "However it needs the power of 50 friends to activate",
        "So go on! 'Befriend' 50 of these lesser creatures, and yes, I obivously mean murder them. Slaughter them all!",
        "As you bring them back to life I'll consider them to be your 'friends'",
        "Robert. Out."
    };

    void Start()
    {
        InputHandler.onInteract += levelOneSpeech;
        Resources.UnloadUnusedAssets();
    }
    
    #region Level 1 Robert
    public void levelOneSpeech()
    {
        if (robertSpeechIndex == 6)
        {
            InputHandler.onInteract -= levelOneSpeech;
            robertUI.SetActive(false);
            robert.SetActive(false);
            healthBar.SetActive(true);
            friendsBar.SetActive(true);
            foreach (GameObject spawner in GameManager.Instance.spawners)
            {
                spawner.SetActive(true);
            }
        }
        if (robertSpeechIndex < 6)
        {
            textRobert.text = robertSpeech[robertSpeechIndex];
        }

        robertSpeechIndex++;
    }
    
    #endregion
}
