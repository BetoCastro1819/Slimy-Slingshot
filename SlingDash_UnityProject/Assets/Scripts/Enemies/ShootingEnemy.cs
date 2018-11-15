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

		//main = chargeBulletEffect.main;
		//main.startLifetime = fireRate;

        timer = 0;
        playerPos = GameManager.GetInstance().player.transform;
		killed = false;
    }
	
	override public void Update ()
    {
        base.Update();

		if (!killed)
		{
			ShootBullet();
			AimTowardsPlayer();
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
				/*
				new Vector2(
				transform.position.x - player.transform.position.x,
				transform.position.y - player.transform.position.y
			);
			*/
			transform.up = dir;
		}
	}
}
