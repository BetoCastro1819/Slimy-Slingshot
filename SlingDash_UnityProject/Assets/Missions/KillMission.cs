using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMission : Mission {

    public int killsToComplete = 2;
    int killCount = 0;

    private void Start()
    {
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
    }

    public override bool IsComplete()
    {
        if (killCount >= killsToComplete)
            return true;

        return false;
    }

    private void Enemy_OnEnemyKilled(Enemy enemy)
    {
        killCount++;
    }
}
