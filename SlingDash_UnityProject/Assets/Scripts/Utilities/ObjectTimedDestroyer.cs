using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTimedDestroyer : MonoBehaviour
{
	public float timeToDestroyObject = 2f;

	void Start ()
	{
		Destroy(gameObject, timeToDestroyObject);
	}
}
