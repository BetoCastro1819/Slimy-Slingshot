using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public GameObject enemyBullet;
    public Transform bulletsParent;
    public Transform shootinPoint;
    public float fireRate = 1f;

    private Player player;
    private float timer;

    override public void Start()
    {
        base.Start();
        timer = fireRate;
        player = FindObjectOfType<Player>();
    }
	
	override public void Update ()
    {
        base.Update();

        timer += Time.deltaTime;
        if (timer > fireRate)
        {
            GameObject bullets = Instantiate(enemyBullet, shootinPoint.position, shootinPoint.rotation);
            bullets.transform.parent = bulletsParent;
            timer = 0;
        }

        if (player != null)
        {
            Vector2 dir = new Vector2(
                transform.position.x - player.transform.position.x,
                transform.position.y - player.transform.position.y
            );
            transform.up = -dir;
        }
	}
}
