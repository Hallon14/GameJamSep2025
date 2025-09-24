using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    [SerializeField]
    private Animator levelTransition;
    [SerializeField]
    private GameObject gameOverUIElement;


    public GameObject portal;
    private int totalFriends;
    [SerializeField]
    private int activeFriends;
    public float playerHealth = 100;
    public List<GameObject> spawners = new List<GameObject>();

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

    public void InitLevel(LevelData levelData)
    {
        portal = levelData.portal;
        levelTransition = levelData.levelTransition;
        spawners = levelData.spawners;
    }

    #region Main Menu and Victory Screen buttons
    public void play()
    {
        // SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
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

    public void updatePlayerHealth(float healthValue)
    {
        playerHealth = healthValue;
    }

    //funcitons to calc active friends
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

    //Checks wincondition, called everytime we update amount of active friends
    public void checkWinCondition(int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                if (activeFriends >= 50)
                {
                    portal.SetActive(true);
                }
                break;
            case 2:
                if (activeFriends >= 100)
                {
                    portal.SetActive(true);
                }
                break;
        }

    }
    public void gameOver()
    {
        pauseGame();
        gameOverUIElement.SetActive(true);
        Instantiate(gameOverUIElement, new Vector3(960,540,0), Quaternion.identity);

    }
    
    #endregion
}