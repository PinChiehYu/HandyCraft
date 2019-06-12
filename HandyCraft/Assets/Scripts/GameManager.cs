using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static object _oLock = new object();
    private static GameManager _instance;

    private ViveInput viveInput;
    public GlobalPPController PPController;
    public WeapondManager WeapondManager;

    public bool FreezeGame { get; private set; }

    public event Action OnWin;
    public event Action OnLose;

    public static GameManager Instance
    {
        get
        {
            lock (_oLock)
            {
                if (_instance == null)
                {
                    Debug.Log("GameManager Instanciated");
                    _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
                    if (_instance == null)
                    {
                        GameObject manager = new GameObject("GameManager");
                        _instance = manager.AddComponent<GameManager>();
                        DontDestroyOnLoad(manager);
                    }
                }
                return _instance;
            }
        }
    }

    private void Awake()
    {
        viveInput = GetComponent<ViveInput>();
        PPController = GetComponent<GlobalPPController>();
        FreezeGame = false;
    }

    void Start()
    {
        if (Instance != this) Destroy(this);
    }

    public IInput GetInputSource()
    {
        return viveInput;
    }

    public void Win()
    {
        FreezeGame = true;
        OnWin?.Invoke();
    }

    public void Lose()
    {
        FreezeGame = true;
        OnLose?.Invoke();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
