﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	public AudioClip onCollisionSound;
	public GameObject bulletEffect;
	public GameObject trail;
	public float bulletSpeed = 20f;
	public float outOfBoundsOffset = 2f;

	private Rigidbody2D rb;
	private Camera cam;

	private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
	}

	private void Update()
    {
		float topScreenEdge = cam.transform.position.y + cam.orthographicSize + outOfBoundsOffset;
		float bottomScreenEdge = cam.transform.position.y - cam.orthographicSize - outOfBoundsOffset;

		if (transform.position.y > topScreenEdge || transform.position.y < bottomScreenEdge)
			Destroy(gameObject);

		rb.velocity = (Vector2)transform.up * bulletSpeed * Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
		AudioManager.Instance.PlayAudioClip(onCollisionSound);
		Instantiate(bulletEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
