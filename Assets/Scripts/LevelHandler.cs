using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHanlder : MonoBehaviour
{
    [SerializeField]
    private LevelData _levelData = null;

    [SerializeField]
    private GameObject _gameManagerPrefab = null;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(_gameManagerPrefab);
        }
    }

    public void Start()
    {
        GameManager.Instance.InitLevel(_levelData);
    }

    //Allows us to press the button the game over screen. Couldnt attach a gamemanager since its not active in the hierarchy
    public void callBack2Main()
    {
        GameManager.Instance.back2Main();
    }
}

[System.Serializable]
public class LevelData
{
    //Testing with two variables
    public GameObject portal = null;
    public Animator levelTransition = null;
    public List<GameObject> spawners = new List<GameObject>();
    public Slider healthBar = null;
    public Image portalUI = null;
    public GameObject gameOver = null;
}