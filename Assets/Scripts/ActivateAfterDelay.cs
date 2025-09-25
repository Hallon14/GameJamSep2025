using UnityEngine;
using System.Collections;

// Attach this to ANY always-active GameObject (e.g., a GameManager).
// Assign the target GameObject you want to activate after a delay.
public class ActivateAfterDelay : MonoBehaviour
{
    [Tooltip("The GameObject that should be SetActive(true) after the delay.")]
    public GameObject target;

    [Tooltip("Seconds to wait before activating the target.")]
    public float delay = 10f;

    [Tooltip("If true, do nothing if target is already active when the delay finishes.")]
    public bool onlyIfInactive = true;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("ActivateAfterDelay: No target assigned.", this);
            return;
        }
        StartCoroutine(DoActivate());
    }

    private IEnumerator DoActivate()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);
        if (target != null)
        {
            if (!onlyIfInactive || !target.activeSelf)
                target.SetActive(true);
        }
    }
}
