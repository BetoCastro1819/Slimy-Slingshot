using UnityEngine;

public abstract class Mission : MonoBehaviour
{
	public string missionID;
	[TextArea]
	public string missionDescription;
    public bool isComplete;
    public int coinReward;

    public abstract bool IsComplete();

    //public void Reward() 
	//{
    //    Destroy(gameObject);
    //}
}
