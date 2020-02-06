using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBoss : Enemy //MonoBehaviour
{
	[Header("OnSpawn")]
	public AudioClip onSpawnSound;
	public float verticalDistanceToTravel;
	public float entranceSpeed;

	[Header("Movement")]
	public float lerpMovementSpeed = 2f;
	public float timeToLockPosAndAttack = 2f;
	public float timeToMoveAgain = 3f;

	[Header("Attack")]
    public List<Tentacle> tentacles;
    public float attackDuration = 2f;
	public float headAttackSpeed = 10f;

	[HideInInspector]
    public int criticalPointsQuant;

	private Vector3 targetStartPosition;
    private PlayerSlimy player;
    private bool canBeKilled;
    private float startMovingTimer;
	private float lockPosTimer;
	private bool canMove;

	private TentacleBossStates bossState;
	private enum TentacleBossStates
	{
		ON_SPAWN,
		HORIZONTAL_MOVEMENT,
		TENTACLE_ATTACK,
		HEAD_ATTACK,
		KILLED
	}

	public override void Start()
    {
		base.Start();

		targetStartPosition = new Vector3(transform.position.x, transform.position.y - verticalDistanceToTravel, transform.position.z);
		//levelManager = LevelManager.GetInstance();
        player = FindObjectOfType<PlayerSlimy>();
        criticalPointsQuant = tentacles.Count;
        canBeKilled = false;
		lockPosTimer = 0;
		startMovingTimer = 0;

		AudioManager.Instance.PlayAudioClip(onSpawnSound);

		bossState = TentacleBossStates.ON_SPAWN;
		canMove = true;
	}

	public override void Update()
    {
		base.Update();

		if (player != null && player.enabled)
			UpdateBossState();
	}

	void UpdateBossState()
	{
		if (criticalPointsQuant <= 0)
			bossState = TentacleBossStates.HEAD_ATTACK;
		
		switch (bossState)
		{
			case TentacleBossStates.ON_SPAWN:
				OnSpawn();
				break;

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

	void OnSpawn()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetStartPosition, entranceSpeed * Time.deltaTime);

		if (transform.position == targetStartPosition)
			bossState = TentacleBossStates.HORIZONTAL_MOVEMENT;
	}

	void HorizontalMovement()
	{
		if (canMove)
		{
			Vector2 smoothMovement = Vector2.Lerp(transform.position,                                               // Boss Position
												  new Vector2(player.transform.position.x, transform.position.y),   // Lerp to player X axis position
												  lerpMovementSpeed * Time.deltaTime);                              // Lerp speed


			// Apply smooth movement
			transform.position = smoothMovement;

			// ATTACK
			lockPosTimer += Time.deltaTime;
			if (lockPosTimer >= timeToLockPosAndAttack)
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

				lockPosTimer = 0;
			}
		}
		else
		{
			startMovingTimer += Time.unscaledDeltaTime;
			if (startMovingTimer >= timeToMoveAgain)
			{
				canMove = true;
				startMovingTimer = 0;
			}
		}
	}

	void TentacleAttack()
	{
		int randomTentacle = Random.Range(0, tentacles.Count);
		Debug.Log("Tentacle index selected: " + randomTentacle);

		if (tentacles[randomTentacle].IsAlive())
		{
			tentacles[randomTentacle].Attack();
		}

		canMove = false;
		bossState = TentacleBossStates.HORIZONTAL_MOVEMENT;
	}

	void HeadAttack()
	{
		canBeKilled = true;
		gameObject.layer = LayerMask.NameToLayer("Boss");

		transform.position = Vector3.Lerp(transform.position, player.transform.position, headAttackSpeed * Time.deltaTime);
	}

	public override void OnCollisionEnter2D(Collision2D collision)
    {
		base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "PlayerBullet")
        {
            if (canBeKilled)
            {
				Rigidbody2D rb = GetComponent<Rigidbody2D>();
				rb.constraints = RigidbodyConstraints2D.None;

				BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
				if (boxCollider)
					boxCollider.enabled = false;

				bossState = TentacleBossStates.KILLED;
            }
        }

		if (collision.gameObject.tag == "Player")
		{
			player.Kill();
		}
	}
}
