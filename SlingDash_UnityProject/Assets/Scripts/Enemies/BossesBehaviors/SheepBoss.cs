using System;
using UnityEngine;

public class SheepBoss : Enemy 
{
	public Action<int> LostHealth_Event;
	public Action ArrivedToPosition_Event;

	[Header("Shooting Points")]
	public Transform shootingPointTop;
	public Transform shootingPointBottom;
	public Transform shootingPointLeft;
	public Transform shootingPointRight;

	[Header("OnEntrance")]
	public AudioClip onSpawnSound;
	public float entranceSpeed;
	public float verticalDistanceToTravel;

	[Header("OnHoldPosition")]
	public float holdPositionForSeconds;

	[Header("OnAttack")]
	public float movementSpeed = 10f;
    public float shootingRate = 2f;
    public GameObject bossBulettPrefab;

	[Header("OnKilled")]
	public AudioClip onKilledSound;

	private PlayerSlimy player;
	private Vector3 targetStartPosition;
    private float timer;
	private bool canTakeDamage;

	private SheepBossState bossState;
	private enum SheepBossState
	{
		ON_SPAWN,
		ON_HOLD_POSITION,
		ON_ATTACK,
		KILLED
	}

    public override void Start()
    {
		base.Start();

		targetStartPosition = new Vector3(transform.position.x, transform.position.y - verticalDistanceToTravel, transform.position.z);

		player = FindObjectOfType<PlayerSlimy>();

        timer = 0;

		canTakeDamage = false;

		bossState = SheepBossState.ON_SPAWN;
		AudioManager.Instance.PlayAudioClip(onSpawnSound);
    }

    public override void Update()
    {
		base.Update();

		UpdateState();
    }

	public void UpdateState()
	{
		switch(bossState)
		{
			case SheepBossState.ON_SPAWN:
				OnSpawn();
				break;
			case SheepBossState.ON_HOLD_POSITION:
				OnHoldPosition();
				break;
			case SheepBossState.ON_ATTACK:
				OnAttack();
				break;
			case SheepBossState.KILLED:
				break;
		}
	}

	public void OnSpawn()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetStartPosition, entranceSpeed * Time.deltaTime);

		if (transform.position == targetStartPosition)
		{
			bossState = SheepBossState.ON_HOLD_POSITION;
			ArrivedToPosition_Event();
		}
	}

	private void OnHoldPosition()
	{
		timer += Time.deltaTime;
		if (timer >= holdPositionForSeconds)
		{
			bossState = SheepBossState.ON_ATTACK;
			canTakeDamage = true;
			timer = 0;
		}
	}

	public void OnAttack()
	{
		UpdateShooting();
		UpdateMovement();

		if (health <= 0)
		{
			bossState = SheepBossState.KILLED;
			AudioManager.Instance.PlayAudioClip(onKilledSound);
		}
	}

	public void UpdateMovement()
	{
		Vector2 dirToPlayer = player.transform.position - transform.position;
		transform.position += (Vector3)(dirToPlayer.normalized * movementSpeed * Time.deltaTime);
	}

	public void UpdateShooting()
	{
		timer += Time.deltaTime;
		if (timer >= shootingRate)
		{
			Fire();
			timer = 0;
		}
	}

    public void Fire() 
	{
		GameObject bullet;

        bullet = Instantiate(bossBulettPrefab, shootingPointTop.position, shootingPointTop.rotation);
		bullet.GetComponent<TrackerBullet>().SetPlayerTransform(player.transform);

        bullet = Instantiate(bossBulettPrefab, shootingPointBottom.position, shootingPointBottom.rotation);
		bullet.GetComponent<TrackerBullet>().SetPlayerTransform(player.transform);

		bullet = Instantiate(bossBulettPrefab, shootingPointLeft.position, shootingPointLeft.rotation);
		bullet.GetComponent<TrackerBullet>().SetPlayerTransform(player.transform);

		bullet = Instantiate(bossBulettPrefab, shootingPointRight.position, shootingPointRight.rotation);
		bullet.GetComponent<TrackerBullet>().SetPlayerTransform(player.transform);
	}

	public override void OnCollisionEnter2D(Collision2D other) 
	{
		if (other.collider.CompareTag("PlayerBullet"))
		{
			AudioManager.Instance.PlayAudioClip(onHitSound);

			if (canTakeDamage)
				TakeDamage(1);

			if (health <= 0)
				KillEnemy();

			LostHealth_Event(health);
		}
	}
}
