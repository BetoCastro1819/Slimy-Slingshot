using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Player player = collision.gameObject.GetComponent<Player>();
		if (player != null)
		{
			Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
			rb.velocity = Vector2.zero;

			player.playerState = Player.PlayerState.ON_DASH;

			Destroy(gameObject);
		}
	}
}
