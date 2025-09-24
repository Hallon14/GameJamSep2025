using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator levelTransition;


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
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
    }

    IEnumerator loadLevel(int levelIndex)
    {
        levelTransition.SetTrigger("Start");

        //Match with transition time
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }
    
    #endregion
}