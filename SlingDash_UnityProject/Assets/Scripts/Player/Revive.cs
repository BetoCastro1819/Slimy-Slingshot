using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
	public GameObject clearScreenTrigger;
	public GameObject reviveParticles;
	public float timeOffsetToRevivePlayer = 0.1f;

	private MeterDetector meterDetector;
	private bool particlesInstantiated;
	private bool playerWantsToRevive;
	private float ReviveY;

	private float timeToRevivePlayer;
	private float timer;

	private Vector3 revivePos;

	private void Start()
    {
        meterDetector = FindObjectOfType<MeterDetector>();
		playerWantsToRevive = false;
		particlesInstantiated = false;

		timeToRevivePlayer = reviveParticles.GetComponent<ParticleSystem>().main.startLifetimeMultiplier;

	}

    private void Update()
    {
        ReviveY = meterDetector.GetMetersTravelled();

		if (playerWantsToRevive)
		{
			if (!particlesInstantiated)
			{
				Instantiate(reviveParticles, revivePos, Quaternion.identity);
				Instantiate(clearScreenTrigger, revivePos, Quaternion.identity);

				particlesInstantiated = true;
			}

			timer += Time.deltaTime;
			if (timer > timeToRevivePlayer + timeOffsetToRevivePlayer)
			{
				RevivePlayer();
			}
		}
    }

	private void RevivePlayer()
	{
		PlayerSlimy player = GameManager.GetInstance().player;

		player.gameObject.SetActive(true);
		player.transform.SetPositionAndRotation(revivePos, Quaternion.identity);
		player.health = 1;

		player.StateMoving.Enter();
		player.SetState(player.StateMoving);

		playerWantsToRevive = false;
		particlesInstantiated = false;

		timer = 0;

		GameManager.GetInstance().SetState(GameManager.GameState.ON_START);
	}

    public void SetRevivePlayerPosition(Vector3 _revivePos)
    {
		revivePos = _revivePos;
		playerWantsToRevive = true;

        // Creates a trigger the bigger than the screen
        // Destroys every object that has the "Erasable" component
		//Instantiate(clearScreenTrigger, revivePos, Quaternion.identity);
	}

	public float GetReviveY()
    {
        return ReviveY;
    }
}
