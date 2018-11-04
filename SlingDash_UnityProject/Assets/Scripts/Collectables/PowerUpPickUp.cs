using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
	public GameObject pickUpEffect;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerSlimy player = collision.gameObject.GetComponent<PlayerSlimy>();
		if (player != null)
		{
			Instantiate(pickUpEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
