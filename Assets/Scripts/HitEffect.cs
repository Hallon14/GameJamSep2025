using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class HitEffect : MonoBehaviour
{
    private SpriteRenderer sprite;

    public float transitionTime = 2f;
    private float timer;
    private float lerpTimer;
    Color color = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // sprite = GetComponent<SpriteRenderer>();
        // timer = transitionTime;
        // lerpTimer = 1f - ((transitionTime - timer) / transitionTime);
        // color = Color.white;
        // StartCoroutine(SpriteColorEffect());
    }

    public virtual void TriggerEffect()
    {

    }

    public IEnumerator SpriteColorEffect()
    {

        float transitionTime = 2f;
        float timer = transitionTime;
        float lerpTimer = 1f - ((transitionTime - timer) / transitionTime);
        Color color = Color.white;
        do
        {
            color = Color.Lerp(color, Color.black, lerpTimer);
            sprite.color = color;
            timer -= Time.deltaTime;
            yield return null;

        } while (timer <= transitionTime);
        color = Color.white;
        yield return null;
    }

    public void ResetTransition()
    {
        timer = transitionTime;
        lerpTimer = 1f - ((transitionTime - timer) / transitionTime);
        color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
