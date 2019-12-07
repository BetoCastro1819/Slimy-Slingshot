using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
	//public ParticleSystem chargeBulletEffect;
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

        timer = 0;

        //playerPos = GameManager.GetInstance().player.transform;

		playerPos = FindObjectOfType<PlayerSlimy>().transform;

		killed = false;
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
			timer = 0;
			//chargeBulletEffect.Play();
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
