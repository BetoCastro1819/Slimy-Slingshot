using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int health = 1;
	public int rechargeEnergyBarValue = 40;
	public float speed = 5f;
	public float offBoundsOffset = 2f;
    public int scoreValue = 50;

	private Camera cam;
	protected float offBounds;

	public virtual void Start()
	{
		cam = Camera.main;
		offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset;
	}

	public virtual void Update()
	{
		offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset;

		if (health <= 0)
			KillEnemy();

			if (transform.position.y < offBounds)
				Destroy(gameObject);
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
	}

	public virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "PlayerBullet")
		{
			TakeDamage(1);
		}
	}

	private void KillEnemy()
	{
        // Camera shake
        ScoreManager.Get().AddScore(scoreValue);
        CoinManager.Get().AddCoins(Random.Range(1,4));
        UI_Manager.Get().scoreText.text = ScoreManager.Get().GetScore().ToString("0000") + "p";
        UI_Manager.Get().coinsText.text = CoinManager.Get().GetCoins().ToString("0000") + "c";
        Destroy(gameObject);
	}
}
