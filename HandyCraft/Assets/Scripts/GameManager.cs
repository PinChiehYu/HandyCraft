using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static object _oLock = new object();
    private static GameManager _instance;

    private ViveInput viveInput;
    public WeapondManager WeapondManager;

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
    }

    void Start()
    {
        if (Instance != this) Destroy(this);
    }

    public IInput GetInputSource()
    {
        return viveInput;
    }

    public void test()
    {
        Debug.Log("TEST");
    }
}
