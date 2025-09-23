using UnityEngine;

public class portal : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.levelComplete();
        }
    }
}