using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class level2Manager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Image portraitPicture;
    public GameObject dialougeBox;
    public GameObject robert;
    public GameObject anton;
    public Sprite antonSprite;
    public Sprite robertSprite;
    //public List<Sprite> portrait = new List<Sprite>();

    private int dialougeIndex;
    List<String> lines = new List<String>()
    {
        "Lets gather up some more! You know, humans commonly use the phrase the more the merrier.",
        "Oh! Hi! Sir! Sir! Sir! You are sooooooo coool!!!!",
        "Fwends are awesome!",
        "I like fwends!",
        "Get more friends!",
        "Anton want more fwends!\nAnton need more fwends!\nAnton no like being alone..",
        "Sorry about that your low-ness,\n ...i mean, your evil-ness \n... i mean, your impeccable-super-awesomeness",
        "Anyway... heh... \nOff you go to befriend some more creatures. I'll make sure Anton doesn't bother you again."
    };

    void Start()
    {
        dialougeIndex = 0;
        InputHandler.onInteract += levelTwoSpeech; 
    }

    public void levelTwoSpeech()
    {
        //Swaps to anton's sprite 
        if (dialougeIndex == 1)
        {
            portraitPicture.sprite = antonSprite;
        }
        //Swaps back to Roberts sprite
        if (dialougeIndex == 6)
        {
            portraitPicture.sprite = robertSprite;
        }

        if (dialougeIndex < 8)
        {
            textComponent.text = lines[dialougeIndex];
        }

        if (dialougeIndex == 8)
        {
            dialougeBox.SetActive(false);
            robert.SetActive(false);
            anton.SetActive(false);
            InputHandler.onInteract -= levelTwoSpeech;
        }

        dialougeIndex++;
    }

}
