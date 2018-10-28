using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ReviveScreen : MonoBehaviour
{
	public MeterDetector meterDetector;
	public CoinManager coinManager;

	public Text metersTravelled;
	public Text	coinsEarned;

	// This value will determine how fast "coins" and "meters"
	// reach their max value on display
	public int reachMaxValueSpeed;
	public float timeBeforeStartCounting;

	private int previousCoinsEarned;
	private float previousMetersReached;
	private float timer;

	private bool doneCountingMeters;

	void Start ()
	{
		previousMetersReached = meterDetector.GetMetersTravelled();
		metersTravelled.text = previousMetersReached.ToString("0m");

		previousCoinsEarned = coinManager.GetCoins();
		coinsEarned.text = previousCoinsEarned.ToString("0");

		timer = 0;
		doneCountingMeters = false;
	}
	
	void Update ()
	{
		if (previousCoinsEarned != coinManager.GetCoins())
		{
			previousCoinsEarned = coinManager.GetCoins();
			coinsEarned.text = previousCoinsEarned.ToString("0");
			Debug.Log("Coins Earned" + previousCoinsEarned);
		}

		if (previousMetersReached != meterDetector.GetMetersTravelled())
		{
			previousMetersReached = meterDetector.GetMetersTravelled();
			metersTravelled.text = previousMetersReached.ToString("0m");
			Debug.Log("Meters Travelled: " + previousMetersReached);
		}
	}
}
