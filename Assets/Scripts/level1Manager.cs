using UnityEngine;

public class level1Manager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject portal;

    void Awake()
    {
    }

    public void setPortalActive()
    {
        portal.SetActive(true);
    }
}
