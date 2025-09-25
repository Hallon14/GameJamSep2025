using UnityEngine;

public class ButtonManager : MonoBehaviour
{
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
}
