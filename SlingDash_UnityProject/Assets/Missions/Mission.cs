using UnityEngine;

public abstract class Mission : MonoBehaviour
{
	public string missionID;
	public string missionDescription;
    public bool isComplete;
    public int coinReward;

    public abstract bool IsComplete();

    public void Reward() 
	{
        CoinManager.Get().AddCoins(coinReward);
        Destroy(gameObject);
    }

    private void Update()
    {
        //isComplete = IsComplete();
    }
}
