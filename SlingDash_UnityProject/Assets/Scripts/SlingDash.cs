using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingDash : MonoBehaviour
{
	public float slinghsotForce = 100f;

	Rigidbody2D rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
		if (Input.GetKey(KeyCode.Mouse0))
			SetDirection();

		if (Input.GetKeyUp(KeyCode.Mouse0))
			Slingshot();

	}

	void SetDirection()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		Vector2 dir = new Vector2(
			mousePos.x - transform.position.x,
			mousePos.y - transform.position.y
		);

		rb.velocity = Vector2.zero;
		transform.up = -dir;
	}

	void Slingshot()
	{
		rb.AddForce(transform.up * slinghsotForce);
	}
}
