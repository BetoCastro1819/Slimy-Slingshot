using UnityEngine;

public class MaxSlingshotsMission : Mission 
{
	[SerializeField] int maxSlingshotsToComplete;

	int slignshotCounter;

	private void Start()
	{
		PlayerSlimy.OnSlingshotCounterIncreased_Event += OnSlingshotCounterIncreased;
	}

	public override bool IsComplete()
	{
		isComplete = (slignshotCounter <= maxSlingshotsToComplete);
		return isComplete;
	}

	private void OnSlingshotCounterIncreased(int slignshotCounter)
	{
		this.slignshotCounter = slignshotCounter;
	}
}
