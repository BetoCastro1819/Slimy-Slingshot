using UnityEngine;

public class WindHazard : MonoBehaviour 
{
	[SerializeField] float windForce;

	private AudioSource audioSource;
	private AudioManager audioManager;

	private void Start()
	{
		audioManager = AudioManager.Instance;
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (audioManager.sfxAreEnabled)
		{
			if (!audioSource.isPlaying)
				audioSource.Play();

			audioSource.volume = audioManager.musicVolume;
		}
		else
		{
			audioSource.Stop();
		}
	}

	private void OnTriggerStay2D(Collider2D other) 
	{
		Rigidbody2D rigidbody2D = other.GetComponent<Rigidbody2D>();
		if (rigidbody2D)
			rigidbody2D.AddForce(-transform.up * windForce);
	}
}
