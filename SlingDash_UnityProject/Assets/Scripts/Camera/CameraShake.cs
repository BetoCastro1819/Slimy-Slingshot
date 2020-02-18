using UnityEngine;

public class CameraShake : MonoBehaviour
{
	#region Singleton
	public static CameraShake Instance { get; private set; }
	void Awake()
	{
		Instance = this;
	}
	#endregion

	public float duration = 0.2f;
	public float magnitude = 0.2f;

	private Vector3 originalPos;
	private bool startShake;
	private float timer = 0f;

	private void Start()
	{
		originalPos = transform.localPosition;
		timer = 0f;
		startShake = false;
	}

	private void Update()
	{
		if (startShake)
		{
			Shake();
		}
	}

	private void Shake()
	{
		timer += Time.unscaledDeltaTime;
		if (timer < duration)
		{
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;

			transform.localPosition = new Vector3(x, y, originalPos.z);
		}
		else
		{
			timer = 0;
			transform.localPosition = originalPos;
			startShake = false;
		}
	}

	public void StartShake()
	{
		startShake = true;
	}
}
