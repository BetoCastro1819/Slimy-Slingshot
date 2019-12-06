using System;
using UnityEngine;

public class Star : MonoBehaviour 
{
	public int starID { get; set; }

	public static event Action<int> OnStarPickedUp_Event;

	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.CompareTag("Player"))
		{
			OnStarPickedUp_Event(starID);
			Destroy(gameObject);
		}	
	}
}
