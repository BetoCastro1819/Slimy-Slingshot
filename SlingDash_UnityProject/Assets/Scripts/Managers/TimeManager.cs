using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	#region Singleton
	private static TimeManager instance;
	public static TimeManager GetInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
	}
	#endregion

	public float bulletTimeFactor = 0.1f;
	public float lerpBulletTime = 15f;

	public enum TimeScales
	{
		DEFAULT,
		SLOW_MO
	}

	public void SetTime(TimeScales timeScale)
	{
		switch (timeScale)
		{
			case TimeScales.DEFAULT:
				DefaultTimeScale();
				break;
			case TimeScales.SLOW_MO:
				SlowMoTimeScale();
				break;
		}
	}

	void SlowMoTimeScale()
	{
		// Smooth transition from normal to slow-mo timeScale
		Time.timeScale = Mathf.Lerp(Time.timeScale, bulletTimeFactor, lerpBulletTime * Time.deltaTime);
		Time.fixedDeltaTime = Time.timeScale * .02f; // 1/50 = 0.02 Assuming game runs at a fixed rate of 50fps
	}

	void DefaultTimeScale()
	{
		// Resets timeScales to default
		Time.timeScale = 1;
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}
}
