using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameData
{
	private int timesPlayedValue;
	private string timesPlayedKey = "TimesPlayed";

	public PersistentGameData()
	{
		if (PlayerPrefs.HasKey(timesPlayedKey))
		{
			timesPlayedValue = PlayerPrefs.GetInt(timesPlayedKey);
		}
		else
		{
			timesPlayedValue = 0;
			PlayerPrefs.SetInt(timesPlayedKey, timesPlayedValue);
		}

		Debug.Log("Times played: " + timesPlayedValue);
	}

	public void AddOneToTimesPlayed()
	{
		timesPlayedValue++;
		PlayerPrefs.SetInt(timesPlayedKey, timesPlayedValue);
	}
}
