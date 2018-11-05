using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEnemy : Enemy
{
	public float attractionForce = 50f;
	public float rotationSpeed = 1f;

	private Rigidbody2D playerRb;

	public override void Update ()
	{
		base.Update();

		transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Debug.Log("Player is triggering");

			Vector2 dir = transform.position - collision.transform.position;

			playerRb.AddForce(dir.normalized * attractionForce * Time.deltaTime);
		}
	}
}
