using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	[SerializeField] AudioSource musicSource;

	public bool musicIsEnabled { get; private set; }
	public bool sfxAreEnabled { get; private set; }

	public static AudioManager Instance { get; private set; }

	private void Awake()
	{
		musicIsEnabled = false;
		sfxAreEnabled = false;

		AudioManager[] audioManager = FindObjectsOfType<AudioManager>();
		if (audioManager.Length == 1)
		{
			Instance = this;

			musicIsEnabled = true;
			sfxAreEnabled = true;

			DontDestroyOnLoad(gameObject);
		}
	}

	public void ToggleMusic()
	{
		musicIsEnabled = !musicIsEnabled;
		if (musicIsEnabled)
			musicSource.Play();
		else
			musicSource.Stop();
	}
}
