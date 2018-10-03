using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    #region Singleton
    private static ScoreManager instance;
    public static ScoreManager Get()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    int score = 0;

    public int GetScore() {
        return score;
    }

    public void AddScore(int _score) {
        score += _score;
    }
}
