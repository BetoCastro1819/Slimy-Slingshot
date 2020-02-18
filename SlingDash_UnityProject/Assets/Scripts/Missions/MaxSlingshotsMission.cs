using UnityEngine;

public class MaxSlingshotsMission : Mission 
{
	[SerializeField] int maxSlingshotsToComplete;

	int slignshotCounter;

	PlayerSlimy player;

	private void Start()
	{
		player = FindObjectOfType<PlayerSlimy>();
		if (player)
			player.OnSlingshotCounterIncreased_Event += OnSlingshotCounterIncreased;
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
