using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject credit;

    public void play()
    {
        GameManager.Instance.play();
    }
    public void nextLevel()
    {
        GameManager.Instance.levelComplete();
    }
    public void back2Main()
    {
        GameManager.Instance.back2Main();
    }
    public void exitGame()
    {
        GameManager.Instance.exit();
    }

    public void creditsButton()
    {
        if (credit.activeSelf)
        {
            credit.SetActive(false);
        }
        else
        {
            credit.SetActive(true);
        }
    }
}
