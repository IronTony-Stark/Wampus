using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Player player;

    private MapGenerator mapGenerator;
    private UIManager uiManager;

    private int stepCount = 0;

    //-------------------------------------------------------------------
    // singleton implementation
    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    //-------------------------------------------------------------------
    // function definitions

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);

            mapGenerator = GetComponent<MapGenerator>();
            uiManager = GetComponent<UIManager>();
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }


    void FixedUpdate()
    {
        if (player.IsGameEnd)
        {
            player.IsGameEnd = false;

            if (player.IsWin)
            {
                uiManager.AddToScore(100);
            }
            else if (player.IsLoose)
            {
                uiManager.ReduceScore(100);
            }

            StartCoroutine(NextLevel());
        }

        if (stepCount != player.StepCount)
        {
            uiManager.ReduceScore(player.StepCount - stepCount);
            stepCount = player.StepCount;
        }
    }


    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1);

        player.ResetPlayer();

        mapGenerator.GenerateWorld();

        uiManager.NextLevel();

        stepCount = 0;
    }
}
