using UnityEngine;
using System.Collections;

// Attach this to an EMPTY prefab (container). Assign the individual frame prefabs (or sprite-objects)
// you want to show in order when an archer dies. Instantiated by ArcherAllyBehavior.Die().
public class SkellyDeath : MonoBehaviour
{
    [Header("Sequence Frames")] 
    [Tooltip("Prefabs shown one after another. Each replaces the previous.")]
    public GameObject[] frames;
    [Tooltip("Seconds each frame stays before switching to the next.")]
    public float frameDuration = 0.18f;
    [Tooltip("Extra time to keep final frame before cleanup if destroyAfter=true.")]
    public float finalHold = 1.5f;
    [Tooltip("Destroy the spawned final frame after the sequence.")]
    public bool destroyAfter = true;
    [Tooltip("If true, leaves the last frame in the scene permanently (overrides destroyAfter).")]
    public bool leaveFinalFrame = false;
    [Tooltip("Auto-destroy this container object at the end (recommended).")]
    public bool destroyContainer = true;

    private GameObject currentInstance;

    private void Start()
    {
        if (frames == null || frames.Length == 0)
        {
            // Nothing to play; just remove container.
            Destroy(gameObject);
            return;
        }
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        for (int i = 0; i < frames.Length; i++)
        {
            var prefab = frames[i];
            if (prefab != null)
            {
                // Spawn new frame
                GameObject newFrame = Instantiate(prefab, transform.position, transform.rotation, transform);
                // Remove previous frame instance to simulate animation change
                if (currentInstance != null)
                {
                    Destroy(currentInstance);
                }
                currentInstance = newFrame;
            }
            if (i < frames.Length - 1) // wait only between frames, not after last (handled by finalHold)
            {
                yield return new WaitForSeconds(frameDuration);
            }
        }

        // Final frame hold
        if (finalHold > 0f)
            yield return new WaitForSeconds(finalHold);

        if (leaveFinalFrame)
        {
            // Detach so container can vanish if requested.
            if (currentInstance != null)
                currentInstance.transform.parent = null;
        }
        else if (destroyAfter && currentInstance != null)
        {
            Destroy(currentInstance);
        }

        if (destroyContainer)
            Destroy(gameObject);
    }
}
