using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int health = 1;
    public int scoreValue = 50;
	public int rechargeEnergyBarValue = 40;
	public float speed = 5f;
	public float offBoundsOffset = 2f;
	public float forceWhenKilled = 50f;

	private Camera cam;
	private CameraShake cameraShake;
	private Rigidbody2D rb;

	protected float offBounds;

	public virtual void Start()
	{
		cam = Camera.main;
		cameraShake = cam.GetComponent<CameraShake>();
		offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset;
		rb = GetComponent<Rigidbody2D>();
	}

	public virtual void Update()
	{
		offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset;

		// Check if enemy is below cameras limit
		if (transform.position.y < offBounds)
		{
			Destroy(gameObject);
		}
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

			if (health <= 0)
			{
				KillEnemy();
			}
		}
	}

	private void KillEnemy()
	{
		StartCoroutine(cameraShake.Shake());

		UpdatePlayerScore();

		transform.Rotate(0, 0, 180);

		if (rb != null)
		{
			rb.constraints = RigidbodyConstraints2D.None;
			rb.AddForce(Vector2.up * forceWhenKilled);
		}

		Collider2D collider = GetComponent<Collider2D>();
		if (collider != null)
		{
			collider.enabled = false;
		}

        //Destroy(gameObject);
	}

	private void UpdatePlayerScore()
	{
		ScoreManager.Get().AddScore(scoreValue);
		CoinManager.Get().AddCoins(Random.Range(1, 4));
		UI_Manager.Get().scoreText.text = ScoreManager.Get().GetScore().ToString("0000") + "p";
		UI_Manager.Get().coinsText.text = CoinManager.Get().GetCoins().ToString("0000") + "c";
	}
}
