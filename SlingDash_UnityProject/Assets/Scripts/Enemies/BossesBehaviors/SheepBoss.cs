using UnityEngine;

public class SheepBoss : Enemy 
{
	[Header("Shooting Points")]
	public Transform shootingPointTop;
	public Transform shootingPointBottom;
	public Transform shootingPointLeft;
	public Transform shootingPointRight;

	[Header("OnSpawn")]
	public float entranceSpeed;
	public float verticalDistanceToTravel;

	[Header("OnAttack")]
	public float movementSpeed = 10f;
    public float shootingRate = 2f;
    public GameObject bossBulettPrefab;

	private PlayerSlimy player;
	private Vector3 targetStartPosition;
    private float shootingTimer;

	private SheepBossState bossState;
	private enum SheepBossState
	{
		ON_SPAWN,
		ON_ATTACK,
		KILLED
	}

    public new void Start()
    {
		targetStartPosition = new Vector3(transform.position.x, transform.position.y - verticalDistanceToTravel, transform.position.z);

		player = FindObjectOfType<PlayerSlimy>();

        shootingTimer = 0;
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
			bossState = SheepBossState.ON_ATTACK;
	}

	public void OnAttack()
	{
		UpdateShooting();
		//UpdateMovement();

		if (health <= 0)
			bossState = SheepBossState.KILLED;
	}

	public void UpdateMovement()
	{
		Vector2 dirToPlayer = transform.position - player.transform.position;
		transform.position += (Vector3)(dirToPlayer.normalized * movementSpeed * Time.deltaTime);
	}

	public void UpdateShooting()
	{
		shootingTimer += Time.deltaTime;
		if (shootingTimer >= shootingRate)
		{
			Fire();
			shootingTimer = 0;
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
}
