using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MeterDetector : MonoBehaviour
{
    public static event Action<float> OnMetersTraveled;

    private float metersTravelled;
	private float startingOffset;

	private void Start()
	{
		// Offset to be substracted from current position
		// Allows to start at 0 meters, even when the player is not at position 0 in real world
		startingOffset = transform.position.y;

		// Initialize current meters record at 0
		metersTravelled = 0;
	}

	void Update ()
	{
		// Saves the amount of travelled meters only when it's 
		// higher than the max high previously reached
		if (transform.position.y - startingOffset >= metersTravelled)
		{
			// Store position
			metersTravelled = transform.position.y - startingOffset;

			// Only updates UI when player's beats previous stored record
			//UI_Manager.Get().meterText.text = metersTravelled.ToString("0000") + " m";

            if (OnMetersTraveled != null)
            {
                OnMetersTraveled(metersTravelled);
            }
		}
	}
    
    public float GetMetersTravelled()
	{
        return metersTravelled;
    }
}
