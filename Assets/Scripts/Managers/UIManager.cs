using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreLabel;
    public Text levelLabel;

    private int score;
    private int levelNumber = 1;

    public void AddToScore(int newScore){
        score += newScore;
    }

    public void ReduceScore(int newScore){
        score -= newScore;
    }

    public void NextLevel(){
        ++levelNumber;
    }


    private void Update(){
        scoreLabel.text = "Score: " + score + '$';
        levelLabel.text = "#Level: " + levelNumber;
    }
}
