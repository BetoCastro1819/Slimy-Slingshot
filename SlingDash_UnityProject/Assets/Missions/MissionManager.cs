using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour {

    Mission[] currentMissions;

    // Use this for initialization
    void Start () {
        currentMissions = GetComponentsInChildren<Mission>();
    }

    // Update is called once per frame
    void Update () {

        for (int i = 0; i < currentMissions.Length; i++)
        {
            if (currentMissions[i] != null && currentMissions[i].isComplete)
            {
                //currentMissions[i].Reward();
                Debug.Log(currentMissions[i].gameObject.name + " COMPLETED!");
                currentMissions[i] = null;
            }
        }
	}


}
