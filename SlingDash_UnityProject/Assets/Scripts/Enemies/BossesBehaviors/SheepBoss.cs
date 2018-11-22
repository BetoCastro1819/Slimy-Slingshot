using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBoss : Enemy {

    public GameObject shootingPoint;
    public GameObject spawner_L;
    public GameObject spawner_R;

    public float shootingTime = 2f;
    public float spawningTime = 5f;

    public GameObject bossBulettPrefab;
    public GameObject platformsPrefab;

    private float shootingTimer;
    private float spawningTimer;

    public new void Start()
    {
        shootingTimer = shootingTime;
        spawningTimer = spawningTime;
    }

    public override void Update()
    {
		base.Update();

        shootingTimer += Time.deltaTime;
        spawningTimer += Time.deltaTime;

        if (shootingTimer >= shootingTime)
        {
            Fire();
            shootingTimer = 0;
        }

        if (spawningTimer >= spawningTime)
        {
            SpawnPlatforms();
            spawningTimer = 0;
        }

        if (health <= 0)
        {
            LevelManager.GetInstance().BossIsActive = false;
            Destroy(gameObject);
        }
    }

    public void Fire() {
        GameObject instantiatedbullet = Instantiate(bossBulettPrefab, shootingPoint.transform.position , Quaternion.identity);
        TrackingBullet bullet = instantiatedbullet.GetComponent<TrackingBullet>();
        bullet.SetPlayer(GameManager.GetInstance().player);
    }

    public void SpawnPlatforms() {
        Instantiate(platformsPrefab, spawner_L.transform.position, Quaternion.identity);
        GameObject obj = Instantiate(platformsPrefab, spawner_R.transform.position, Quaternion.identity);
        //obj.GetComponent<MovingPlatform>().SetIsGoingLeft(true);
    }
}
