using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager _Instance;

    //Singleton--------------------------
    private void Awake()
    {
        if(_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    //------------------------------------

    public PoolManager _PoolManager;
    public UiManager _UiManager;
    public Player _Player;

    public GameObject _GoldPrefab;
    public int _Gold;

}
