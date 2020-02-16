using UnityEngine;

public class BossIncomingAudioPlayer : MonoBehaviour 
{
	[SerializeField] AudioClip bossIncomingAudioClip;

	private void Start() 
	{
		AudioManager.Instance.PlayAudioClip(bossIncomingAudioClip);
	}
}
