using System.Collections.Generic;
using UnityEngine;

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
}

[System.Serializable]
public class LevelData
{

    //Testing with two variables
    public GameObject portal = null;
    public Animator levelTransition = null;
    public List<GameObject> spawners = new List<GameObject>();
}