using UnityEngine;

public class Spikes : MonoBehaviour 
{
	private void OnCollisionEnter2D(Collision2D other) 
	{
		PlayerSlimy player = other.gameObject.GetComponent<PlayerSlimy>();
		if (player)
			player.Kill();
	}
}
