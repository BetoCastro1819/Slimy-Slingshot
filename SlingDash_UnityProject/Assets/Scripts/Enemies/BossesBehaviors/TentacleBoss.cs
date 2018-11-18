using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBoss : MonoBehaviour
{
	[Header("Movement")]
	public float lerpMovementSpeed = 2f;
	public float timeToLockPos = 2f;

	[Header("Attack")]
    public List<Tentacle> tentacles;
    public float timeToAttack = 2f;
	public float headAttackSpeed = 10f;

	[HideInInspector]
    public int criticalPointsQuant;

    private LevelManager levelManager;
    private PlayerSlimy player;
    private bool canBeKilled;
    private float attackTimer;
	private float lockPosTimer;

	private TentacleBossStates bossState;
	private enum TentacleBossStates
	{
		HORIZONTAL_MOVEMENT,
		TENTACLE_ATTACK,
		HEAD_ATTACK,
		KILLED
	}

	private void Start()
    {
		bossState = TentacleBossStates.HORIZONTAL_MOVEMENT;

		levelManager = LevelManager.GetInstance();
        player = FindObjectOfType<PlayerSlimy>();
        criticalPointsQuant = tentacles.Count;
        canBeKilled = false;
		lockPosTimer = 0;
	}

    private void Update()
    {
		attackTimer += Time.deltaTime;
		if (attackTimer >= timeToAttack)
		{
			TentacleAttack();
		}

		/*
		if (player != null && player.enabled)
		{
			UpdateBossState();
		}
		
		
		if (criticalPointsQuant > 0)
		{
			attackTimer += Time.deltaTime;
			if (attackTimer >= timeToAttack)
			{
				TentacleAttack();
			}
		}
		else
		{
			HeadAttack();
		}
		*/
	}

	void UpdateBossState()
	{
		switch (bossState)
		{
			case TentacleBossStates.HORIZONTAL_MOVEMENT:
				HorizontalMovement();
				break;

			case TentacleBossStates.TENTACLE_ATTACK:
				TentacleAttack();
				break;

			case TentacleBossStates.HEAD_ATTACK:
				HeadAttack();
				break;

			case TentacleBossStates.KILLED:
				break;
		}
	}

	void HorizontalMovement()
	{
		if (player.enabled)
		{
			Vector2 smoothMovement = Vector2.Lerp(transform.position,                                               // Boss Position
												  new Vector2(player.transform.position.x, transform.position.y),   // Lerp to player X axis position
												  lerpMovementSpeed * Time.deltaTime);                              // Lerp speed

			// Apply smooth movement
			transform.position = smoothMovement;


			// ATTACK
			lockPosTimer += Time.unscaledDeltaTime;
			if (lockPosTimer >= timeToLockPos)
			{
				// Attack with tentacles if there are still any
				if (criticalPointsQuant > 0)
				{
					bossState = TentacleBossStates.TENTACLE_ATTACK;
				}
				else
				{
					bossState = TentacleBossStates.HEAD_ATTACK;
				}
			}
		}
	}

	void TentacleAttack()
	{
		int randomTentacle = Random.Range(0, tentacles.Count);

		if (tentacles[randomTentacle].IsAlive())
		{
			tentacles[randomTentacle].Attack();
		}

		attackTimer = 0;

		/*
		for (int i = 0; i < tentacles.Count; i++)
		{

			if (tentacles[i].IsAlive())
			{
				if (player.enabled)
				{
					//criticalPoints[i].Attack(player.transform.position);

					tentacles[i].Attack();
					attackTimer = 0;
					return;
				}
			}
		}
		*/
	}

	void HeadAttack()
	{
		canBeKilled = true;
		gameObject.layer = LayerMask.NameToLayer("Boss");

		transform.position = Vector3.Lerp(transform.position, player.transform.position, headAttackSpeed * Time.deltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            if (canBeKilled)
            {
                levelManager.BossIsActive = false;

                Destroy(gameObject);
            }
        }

		if (collision.gameObject.tag == "Player")
		{
			player.TakeDamage(10);
		}
	}
}
