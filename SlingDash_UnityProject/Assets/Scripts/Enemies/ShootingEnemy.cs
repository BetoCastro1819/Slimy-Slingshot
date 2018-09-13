using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public GameObject enemyBullet;
    public float fireRate = 1f;

    private float timer;

	void Start ()
    {
        timer = fireRate;
	}
	
	void Update ()
    {
		
	}
}
