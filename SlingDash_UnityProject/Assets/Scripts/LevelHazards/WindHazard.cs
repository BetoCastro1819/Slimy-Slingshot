using UnityEngine;

public class WindHazard : MonoBehaviour 
{
	[SerializeField] float windForce;

	private void OnTriggerStay2D(Collider2D other) 
	{
		Rigidbody2D rigidbody2D = other.GetComponent<Rigidbody2D>();
		if (rigidbody2D)
			rigidbody2D.AddForce(-transform.up * windForce);
	}
}
