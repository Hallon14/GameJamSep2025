using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Main Menu and Victory Screen buttons
    public void play()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void exit()
    {

        Application.Quit();
        //Exits playmode while working in Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    public void back2Main()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
    
    #region In-game Functions
    public void levelComplete()
    {
        //Finds the current scene index and loads the next one
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1, LoadSceneMode.Single);
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
    }
    
    #endregion
}