using System.Collections.Generic;
using UnityEngine;

public class MovingObjectsManager : MonoBehaviour 
{
	[SerializeField] List<MovingObject> movingObjects;
	[SerializeField] float delayBetweenObjects;

	void Awake() 
	{
		float delayAmount = 0;
		for (int i = 0; i < movingObjects.Count; i++)
		{
			movingObjects[i].SetDelay(delayAmount);
			delayAmount += delayBetweenObjects;
		}
	}
}
