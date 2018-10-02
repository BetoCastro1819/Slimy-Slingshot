using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectDestroyer : MonoBehaviour
{
	ParticleSystem particles;

	void Start ()
	{
		particles = GetComponent<ParticleSystem>();
		float timeToDestroy = particles.main.startLifetimeMultiplier;
		Destroy(gameObject, timeToDestroy);
	}
}
