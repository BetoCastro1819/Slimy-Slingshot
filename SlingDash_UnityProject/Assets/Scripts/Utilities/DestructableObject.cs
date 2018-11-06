using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
	public GameObject destroyedObject;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "PlayerBullet")
		{
			// Maybe instantiate some particles too

			Camera.main.GetComponent<CameraShake>().StartShake();

			gameObject.GetComponent<PolygonCollider2D>().enabled = false;

			Instantiate(destroyedObject, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
