using UnityEngine;

public class DestructableObject : MonoBehaviour
{
	public AudioClip branchOnDestroySound;
	public GameObject destroyedObject;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "PlayerBullet")
		{
			Camera.main.GetComponent<CameraShake>().StartShake();
			gameObject.GetComponent<PolygonCollider2D>().enabled = false;
			Instantiate(destroyedObject, transform.position, Quaternion.identity);
			AudioManager.Instance.PlayAudioClip(branchOnDestroySound);
			Destroy(gameObject);
		}
	}
}
