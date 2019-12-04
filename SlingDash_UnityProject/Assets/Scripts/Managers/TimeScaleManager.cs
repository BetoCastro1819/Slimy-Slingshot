using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
	#region Singleton
	public static TimeScaleManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}
	#endregion

	public float bulletTimeFactor = 0.1f;
	public float lerpBulletTime = 15f;

	public void EnableSlowMoTimeScale()
	{
		// Smooth transition from normal to slow-mo timeScale
		Time.timeScale = Mathf.Lerp(Time.timeScale, bulletTimeFactor, lerpBulletTime * Time.deltaTime);
		Time.fixedDeltaTime = Time.timeScale * .02f; // 1/50 = 0.02 Assuming game runs at a fixed rate of 50fps
	}

	public void DisableSlowMoTimeScale()
	{
		// Resets timeScales to default
		Time.timeScale = 1;
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}
}
