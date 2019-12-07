using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKilled;

    public GameObject coinParticleEffect;
	public int health = 1;
    public int scoreValue = 50;
	public int rechargeEnergyBarValue = 40;
	public float speed = 5f;
	public float offBoundsOffset = 2f;
	public float forceWhenKilled = 50f;
	public float flashDurationWhenKilled = 0.2f;

	private Camera cam;
	private CameraShake cameraShake;
	private Rigidbody2D rb;

	protected bool killed;
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
		if (cam != null)
		{
			offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset;
		}
		// Check if enemy is below cameras limit
		if (transform.position.y < offBounds)
		{
			Destroy(gameObject);

			/*
			if (transform.parent.gameObject != null)
			{
				Destroy(transform.parent.gameObject);
			}
			*/
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

	public virtual void KillEnemy()
	{
        if (OnEnemyKilled != null)
        {
            OnEnemyKilled(this);
        }

		//Instantiate(coinParticleEffect, transform.position, Quaternion.identity);

		killed = true;

		if (cameraShake != null)
		{
			cameraShake.StartShake();
		}

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

		Material material = GetComponent<SpriteRenderer>().material;
		material.SetFloat("_FlashAmount", 1.0f);
		Invoke("ResetFlashAmount", flashDurationWhenKilled);


        //Destroy(gameObject);
	}

	private void ResetFlashAmount()
	{
		Material material = GetComponent<SpriteRenderer>().material;
		material.SetFloat("_FlashAmount", 0);
	}

	private void UpdatePlayerScore()
	{
		if (ScoreManager.Get() != null)
		{
			ScoreManager.Get().AddScore(scoreValue);
		}

		if (CoinManager.Get() != null)
		{
			CoinManager.Get().AddCoins(20);
		}

		if (UI_Manager.Get() != null)
		{
			UI_Manager.Get().scoreText.text = ScoreManager.Get().GetScore().ToString("0000") + "p";
		}
	}
}
