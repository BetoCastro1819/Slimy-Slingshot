using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public float duration = 0.2f;
	public float magnitude = 4f;

	public IEnumerator Shake()
	{
		Vector3 originalPos = transform.localPosition;

		float timer = 0f;

		while (timer < duration)
		{
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;

			transform.localPosition = new Vector3(x, y, originalPos.z);

			timer += Time.unscaledDeltaTime;

			yield return null;
		}

		transform.localPosition = originalPos;
	}
}
