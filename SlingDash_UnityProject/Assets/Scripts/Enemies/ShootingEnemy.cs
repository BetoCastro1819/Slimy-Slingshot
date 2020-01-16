using UnityEngine;

public class ShootingEnemy : Enemy
{
    public GameObject enemyBullet;
    public Transform bulletsParent;
    public Transform shootinPoint;
    public float fireRate = 1f;

	private ParticleSystem.MainModule main;
	private Transform playerPos;
    private float timer;

    override public void Start()
    {
        base.Start();

		playerPos = FindObjectOfType<PlayerSlimy>().transform;

		killed = false;
        timer = 0;
    }
	
	override public void Update ()
    {
        base.Update();

		if (!killed)
		{
			AimTowardsPlayer();
			ShootBullet();
		}
	}

	void ShootBullet()
	{
		timer += Time.deltaTime;
		if (timer > fireRate)
		{
			GameObject bullets = Instantiate(enemyBullet, shootinPoint.position, shootinPoint.rotation);
			bullets.transform.parent = bulletsParent;

			TrackerBullet trackerBullet = bullets.GetComponent<TrackerBullet>();
			if (trackerBullet)
				trackerBullet.SetPlayerPosition(playerPos);
		
			timer = 0;
		}
	}

	void AimTowardsPlayer()
	{
		if (playerPos != null)
		{
			Vector2 dir = transform.position - playerPos.position;
			transform.up = dir;
		}
	}

	private void OnDestroy()
	{
		if (transform.parent != null)
		{
			Destroy(transform.parent.gameObject);
		}
	}
}
