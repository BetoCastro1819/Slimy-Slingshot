using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickUp : MonoBehaviour
{
	public int rechargeValue = 20;
	public float posOffsetForDestroy = 2f;

	private Camera cam;

	void Start ()
	{
		cam = Camera.main;
	}
	
	void Update ()
	{
		if (transform.position.y < cam.transform.position.y - cam.orthographicSize - posOffsetForDestroy)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Player player = collision.gameObject.GetComponent<Player>();
			player.RechargeEnergyBar(rechargeValue);

			// Spawn effects for pick up feedback
			Destroy(gameObject);
		}

	}
}
