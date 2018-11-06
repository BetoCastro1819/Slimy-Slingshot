using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedObject : MonoBehaviour
{
	public float torqueForce;

	void Start()
	{
		GetComponent<Rigidbody2D>().AddTorque(torqueForce);
	}
}
