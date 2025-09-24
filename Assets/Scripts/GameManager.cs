using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public Animator levelTransition;
    public GameObject gameOverUIElement;
    private int totalFriends;
    private int activeFriends;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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

    public void pauseGame()
    {
        Time.timeScale = 0f;
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
    }

    public void levelComplete()
    {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator loadLevel(int levelIndex)
    {
        levelTransition.SetTrigger("Start");

        //Match with transition time
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }

    public void increaseFriendCount()
    {
        totalFriends++;
        activeFriends++;
        checkWinCondition(SceneManager.GetActiveScene().buildIndex);

    }
    public void decreaseFriendCount()
    {
        activeFriends--;
        checkWinCondition(SceneManager.GetActiveScene().buildIndex);
    }

    public void checkWinCondition(int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                if (activeFriends >= 50)
                {
                    //Open portal
                }
                break;
            case 2:
                if (activeFriends >= 100)
                {
                    //Open portal
                }
                break;
        }

    }


    public void gameOver()
    {
        pauseGame();
        gameOverUIElement.SetActive(true);

    }
    
    #endregion
}