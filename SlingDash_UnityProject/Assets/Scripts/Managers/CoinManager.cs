using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour {

    #region Singleton
    private static CoinManager instance;
    public static CoinManager Get()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    int coins = 0;

    public int GetCoins() {
        return coins;
    }

    public void AddCoins(int _coins) {
        coins += _coins;
    }
}
