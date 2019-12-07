using System;
using UnityEngine;

public class Star : MonoBehaviour 
{
	[SerializeField] GameObject onPickUpParticleEffect;

	public int starID { get; set; }

	public static event Action<int> OnStarPickedUp_Event;

	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.CompareTag("Player"))
		{
			Instantiate(onPickUpParticleEffect, transform.position, Quaternion.identity);
			OnStarPickedUp_Event(starID);
			Destroy(gameObject);
		}	
	}
}
