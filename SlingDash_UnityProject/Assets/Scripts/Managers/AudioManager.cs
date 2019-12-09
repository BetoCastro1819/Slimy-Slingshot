using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	[SerializeField] AudioSource musicSource;

	public bool musicIsEnabled { get; private set; }
	public bool sfxAreEnabled { get; private set; }

	public static AudioManager Instance { get; private set; }

	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();

		AudioManager[] audioManager = FindObjectsOfType<AudioManager>();
		if (audioManager.Length == 1)
		{
			Instance = this;

			musicIsEnabled = true;
			sfxAreEnabled = true;

			DontDestroyOnLoad(gameObject);
		}
		UpdateMusicState();
	}

	private void UpdateMusicState()
	{
		if (musicIsEnabled)
			musicSource.Play();
		else
			musicSource.Stop();
	}

	public void ToggleMusic()
	{
		musicIsEnabled = !musicIsEnabled;
		UpdateMusicState();
	}

	public void ToggleSfx()
	{
		sfxAreEnabled = !sfxAreEnabled;
	}

	public void PlayAudioClip(AudioClip clip)
	{
		if (sfxAreEnabled)
			audioSource.PlayOneShot(clip, 1);
	}
}
