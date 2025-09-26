using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject credit;
    public GameObject controlScheme;

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

    //Show/Hide Credits
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

    //Show/Hide Controls
    public void controlButton()
    {
        if (controlScheme.activeSelf)
        {
            controlScheme.SetActive(false);
        }
        else
        {
            controlScheme.SetActive(true);
        }
    }
}
