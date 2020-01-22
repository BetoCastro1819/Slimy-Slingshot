using UnityEngine;

public class TrackerBullet : MonoBehaviour 
{
	[SerializeField] GameObject bulletEffect;
	[SerializeField] float speed = 5f;
	[SerializeField] float rotationSpeed = 200f;

	private Transform playerTransform;
	private Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Vector2 desiredDirection = (Vector2)playerTransform.position - rb.position;
		float rotationValue = Vector3.Cross(desiredDirection.normalized, transform.up).z;
		rb.angularVelocity = rotationValue * rotationSpeed;
		rb.velocity = -transform.up * speed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		PlayerSlimy player = collision.gameObject.GetComponent<PlayerSlimy>();
		if (player != null)
		{
			player.Kill();
		}

		Instantiate(bulletEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	public void SetPlayerTransform(Transform playerTransform)
	{
		this.playerTransform = playerTransform;
	}
}
