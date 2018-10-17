using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erasable : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "ClearScreenTrigger")
		{
			Destroy(gameObject);
		}
	}
}