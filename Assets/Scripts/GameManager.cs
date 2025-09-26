using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    //UI Elements
    [SerializeField]
    private Animator levelTransition;
    [SerializeField]
    private GameObject gameOverUIElement;
    [SerializeField]
    private Slider healthBar;
    public Image portalUI;
    public TextMeshProUGUI statText;

    //Gameobjects
    public GameObject portal;
    [SerializeField]
    public float playerHealth = 100;
    public List<GameObject> spawners = new List<GameObject>();

    //winconditions
    public float friendsNeededForLevel1 = 75.0f;
    public float friendsNeededForLevel2 = 150.0f;

    //Friend Variables
    private int totalFriends;
    [SerializeField]
    private float activeFriends;


    //Startup functions. Needed to make sure each level has a gamemanager
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

    //Sets variables for each individual level.
    public void InitLevel(LevelData levelData)
    {
        if (levelData.portal)
        {
            portal = levelData.portal;
        }

        if (levelData.levelTransition)
        {
            levelTransition = levelData.levelTransition;
        }

        if (levelData.spawners.Count > 0)
        {
            spawners = levelData.spawners;
        }

        if (levelData.healthBar)
        {
            healthBar = levelData.healthBar;
        }

        if (levelData.portalUI)
        {
            portalUI = levelData.portalUI;
        }

        if (levelData.gameOver)
        {
            gameOverUIElement = levelData.gameOver;
        }

        if (levelData.statText)
        {
            statText = levelData.statText;
        }

    }

    #region Buttons
    public void play()
    {
        // SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        playerHealth = 100;
        totalFriends = 0;
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
        StartCoroutine(loadLevel(0));
        gameOverUIElement.SetActive(false);
    }

    #endregion

    #region Basic Gamemanager Functions
    //Play, Pause and Game over
    public void pauseGame() {
        Time.timeScale = 0f;
    }

    public void resumeGame() {
        Time.timeScale = 1f;
    }

    public void gameOver()
    {
        statText.text = "During your adventure you collected " + totalFriends + " friends!";
        gameOverUIElement.SetActive(true);
    }
    #endregion

    #region Level Transitions
    /* 
    Helper functions to make scene transitions smooth. Each scene has a transtion gameobject that automaticly plays
    a "fade-in" when the scene is loaded. So all we do it fade out the active scene as it comes to and end
    */

    public void levelComplete()
    {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));

        if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.GetSceneByName("VictoryScreen").buildIndex)
        {
            statText.text = "During your adventure you collected " + totalFriends + " friends!";
        }

        //Reset the portalvalues to 0. So the UI isnt half full when next level begins
        activeFriends = 0;
        updatePortalUI(SceneManager.GetActiveScene().buildIndex + 1);

    }

    //Helper function! Allows us to wait for animations to play before next level start
    IEnumerator loadLevel(int levelIndex)
    {
        levelTransition.SetTrigger("Start");

        //Match with transition time
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }
    #endregion


    #region Updates on events

    public void updatePlayerHealth(float healthValue){
        playerHealth = healthValue;
        healthBar.value = playerHealth / 100;
    }

    //funcitons to calc active friends
    public void increaseFriendCount() {
        totalFriends++;
        activeFriends++;
        checkWinCondition(SceneManager.GetActiveScene().buildIndex);
        updatePortalUI(SceneManager.GetActiveScene().buildIndex);
    }

    public void decreaseFriendCount(){
        activeFriends--;
        checkWinCondition(SceneManager.GetActiveScene().buildIndex);
        updatePortalUI(SceneManager.GetActiveScene().buildIndex);
    }

    public void updatePortalUI(int currentLevel)
    {
        float friendsNeeded = 0;
        if (currentLevel == 1)
        {
            friendsNeeded = friendsNeededForLevel1;
        }
        else {
            friendsNeeded = friendsNeededForLevel2;
        }
        


        if (activeFriends / friendsNeeded > 1)
        {
            portalUI.fillAmount = 1;
        }
        else
        {
            portalUI.fillAmount = activeFriends / friendsNeeded;
        }
    }

    //Checks wincondition, called everytime we update amount of active friends
    public void checkWinCondition(int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                if (activeFriends >= friendsNeededForLevel1)
                {
                    portal.SetActive(true);
                }
                break;
            case 2:
                if (activeFriends >= friendsNeededForLevel2)
                {
                    portal.SetActive(true);
                }
                break;
        }

    }
    #endregion
    
}