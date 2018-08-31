using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingDash : MonoBehaviour
{
	public GameObject aimHandle;
	public float slinghsotForce = 100f;

	Rigidbody2D rb;
	Vector3 mousePos;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		aimHandle.SetActive(false);
	}
	
	void Update ()
	{
		if (Input.GetKey(KeyCode.Mouse0))
			SetDirection();

		if (Input.GetKeyDown(KeyCode.Mouse0))
			CreateAimingHandle();

		if (Input.GetKeyUp(KeyCode.Mouse0))
			Slingshot();
	}

	void SetDirection()
	{
		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		Vector2 dir = new Vector2(
			mousePos.x - aimHandle.transform.position.x,
			mousePos.y - aimHandle.transform.position.y
		);

		rb.velocity = Vector2.zero;
		aimHandle.transform.up = -dir;

		transform.rotation = aimHandle.transform.rotation;

		// If player collides, it keeps rotating after slingshot release
		//rb.rotation = 0;

		// Ask how to make slow mo effect non-gittery
		//Time.timeScale = 0.1f;
	}

	void Slingshot()
	{
		//Time.timeScale = 1f;
		rb.AddForce(transform.up * slinghsotForce);
		aimHandle.SetActive(false);
	}

	void CreateAimingHandle()
	{
		aimHandle.transform.position = new Vector2(mousePos.x, mousePos.y);
		aimHandle.SetActive(true);
	}
}
