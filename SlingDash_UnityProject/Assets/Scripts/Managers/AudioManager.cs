using UnityEngine;

public class AudioManager : Initializable
{
	[SerializeField] float volumeOnPause;
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource ambientSoundsSource;

	public bool musicIsEnabled { get; private set; }
	public bool sfxAreEnabled { get; private set; }

	public static AudioManager Instance { get; private set; }
	public float musicVolume { get { return musicSource.volume; } }

	private AudioSource audioSource;
	private float musicNormalVolume;
	private float sfxNormalVolume;
	private float ambientNormalVolume;

	private bool initialized = false;

	public override void Initialize()
	{
		// Check if there is more than ONE instance of this class
		AudioManager[] audioManager = FindObjectsOfType<AudioManager>();
		if (audioManager.Length != 1) return;

		Instance = this;

		audioSource = GetComponent<AudioSource>();

		musicIsEnabled = PersistentGameData.Instance.gameData.musicIsEnabled;
		sfxAreEnabled = PersistentGameData.Instance.gameData.sfxAreEnabled;

		musicNormalVolume = musicSource.volume;
		sfxNormalVolume = audioSource.volume;
		ambientNormalVolume = ambientSoundsSource.volume;

		DontDestroyOnLoad(gameObject);

		UpdateAmbientSoundState();

		initialized = true;
	}

	public void UpdateAmbientSoundState() 
	{
		if (sfxAreEnabled)
			ambientSoundsSource.Play();
		else
			ambientSoundsSource.Stop();
	}

	public void PauseAmbientSounds()
	{
		ambientSoundsSource.Stop();
	}

	public void UpdateMusicState()
	{
		if (musicIsEnabled)
			musicSource.Play();
		else
			musicSource.Stop();
		
		PersistentGameData.Instance.UpdateMusicToggle(musicIsEnabled);
	}

	public void ToggleMusic()
	{
		musicIsEnabled = !musicIsEnabled;
		UpdateMusicState();
	}

	public void ToggleSfx()
	{
		sfxAreEnabled = !sfxAreEnabled;
		UpdateAmbientSoundState();
		PersistentGameData.Instance.UpdateSfxToggle(sfxAreEnabled);
	}

	public void PlayAudioClip(AudioClip clip)
	{
		if (sfxAreEnabled)
			audioSource.PlayOneShot(clip, 1);
	}

	public void StopMusic()
	{
		musicSource.Stop();
	}

	public void LowerVolumeOnPause()
	{
		musicSource.volume = volumeOnPause;
		audioSource.volume = volumeOnPause;
		ambientSoundsSource.volume = volumeOnPause;
	}

	public void RiseVolumeToGameplayLevel()
	{
		if (!initialized) return;

		musicSource.volume = musicNormalVolume;
		audioSource.volume = sfxNormalVolume;
		ambientSoundsSource.volume = ambientNormalVolume;
	}
}
