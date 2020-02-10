using System.Collections.Generic;
using UnityEngine;

public abstract class Initializable : MonoBehaviour
{
	public abstract void Initialize();
}

public class Initializer : MonoBehaviour 
{
	[SerializeField] List<Initializable> objectsToInitialize;

	private static bool initialized = false;

	void Awake() 
	{
		if (!initialized)
		{
			Debug.Log("Initializing objects");
			for (int i = 0; i < objectsToInitialize.Count; i++)
			{
				objectsToInitialize[i].Initialize();
			}
			initialized = true;
		}
		Debug.Log("Objects already initialized");
	}
}
