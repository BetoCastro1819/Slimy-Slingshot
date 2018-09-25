using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterDetector : MonoBehaviour {

    float maxMeters = 0;
    float meters = 0;

	// Update is called once per frame
	void Update () {
        meters = transform.position.y;

        if (meters >= maxMeters)
        {
            maxMeters = meters;
        }

        if (meters <= 0)
        {
            meters = 0;
        }

        UI_Manager.Get().meterText.text = meters.ToString("0000");
	}
}
