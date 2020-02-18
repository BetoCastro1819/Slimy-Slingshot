using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action OnEnemyKilled;

	public AudioClip onHitSound;
	public GameObject coinParticleEffect;
	public int health = 1;
    public int scoreValue = 50;
	public float speed = 5f;
	public float offBoundsOffset = 2f;
	public float forceWhenKilled = 50f;
	public float flashDurationWhenKilled = 0.2f;

	private Camera cam;
	private CameraShake cameraShake;
	private Rigidbody2D rb;
	private Material material;

	protected bool killed;
	protected float offBounds;

	public virtual void Start()
	{
		cam = Camera.main;
		cameraShake = cam.GetComponent<CameraShake>();
		offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset;
		rb = GetComponent<Rigidbody2D>();
		material = GetComponent<SpriteRenderer>().material;
	}

	public virtual void Update()
	{
		if (cam == null)
			cam = Camera.main;

		if (cam != null)
			offBounds = cam.transform.position.y - cam.orthographicSize - offBoundsOffset;

		if (transform.position.y < offBounds)
		{
			Destroy(gameObject);
		}
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		material.SetFloat("_FlashAmount", 1.0f);
		Invoke("ResetFlashAmount", flashDurationWhenKilled);
	}

	public virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "PlayerBullet")
		{
			AudioManager.Instance.PlayAudioClip(onHitSound);
			
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
            OnEnemyKilled();

		killed = true;

		if (cameraShake != null)
			cameraShake.StartShake();

		transform.Rotate(0, 0, 180);

		if (rb != null)
		{
			rb.constraints = RigidbodyConstraints2D.None;
			rb.AddForce(Vector2.up * forceWhenKilled);
		}

		Collider2D collider = GetComponent<Collider2D>();
		if (collider != null)
			collider.enabled = false;
	}

	private void ResetFlashAmount()
	{
		Material material = GetComponent<SpriteRenderer>().material;
		material.SetFloat("_FlashAmount", 0);
	}
}
