using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class healthBarScript : MonoBehaviour
{

    public List<Sprite> sprites = new List<Sprite>(); // (If still needed for fountain animation)
    public GameObject fountain; // (Optional animated sprite object)
    private int spriteIndex = 1;

    [Header("Health Slider References")] public Slider redSlider; // Assign RedSlider in inspector or auto-find
    [Tooltip("If not assigned, will search child named 'RedSlider'")] public string redSliderChildName = "RedSlider";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (redSlider == null)
        {
            Transform t = transform.Find(redSliderChildName);
            if (t != null)
                redSlider = t.GetComponent<Slider>();
        }

        if (redSlider != null)
        {
            redSlider.minValue = 0f;
            redSlider.maxValue = 1f;
            redSlider.value = 1f; // full at start
        }

        PlayerHealth.onPlayerHPChanged += UpdateHealthBar;

        if (fountain != null && sprites.Count > 0)
        {
            InvokeRepeating(nameof(swapSprite), 0, 1.5F);
        }
    }

    void OnDestroy()
    {
        PlayerHealth.onPlayerHPChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float normalized)
    {
        if (redSlider != null)
        {
            redSlider.value = Mathf.Clamp01(normalized);
        }
    }


    void swapSprite()
    {
        if (fountain == null || sprites.Count == 0) return;
        var img = fountain.GetComponent<Image>();
        if (img == null) return;
        fountain.GetComponent<Image>().sprite = sprites[spriteIndex];
        if (spriteIndex == 1) spriteIndex = 0; else spriteIndex++;
    }
}
