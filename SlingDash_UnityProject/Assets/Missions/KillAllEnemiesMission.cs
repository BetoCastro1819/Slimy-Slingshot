using UnityEngine;

public class KillAllEnemiesMission : Mission 
{
    public override bool IsComplete()
    {
		int enemiesAlive = LevelBased.LevelManager.Instance.numberOfEnemiesInLevel;
		isComplete = (enemiesAlive <= 0); 
		//Debug.Log("Enemies alive: " + enemiesAlive);
        return isComplete;
    }
}
