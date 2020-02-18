using UnityEngine;

public class PortalToNextLevel : MonoBehaviour 
{
	[SerializeField] float rotationSpeed = -50f;
	
	void Update()
	{
		transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
	}
}
