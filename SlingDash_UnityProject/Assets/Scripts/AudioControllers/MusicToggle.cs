using UnityEngine;

public class MusicToggle : MonoBehaviour 
{
	[SerializeField] GameObject musicOn;
	[SerializeField] GameObject musicOff;

	AudioManager audioManager;

	private void Start () 
	{
		audioManager = AudioManager.Instance;
		UpdateButtonUI();
	}

	public void ToggleMusic()
	{
		audioManager.ToggleMusic();
		UpdateButtonUI();
	}

	private void UpdateButtonUI()
	{
		bool musicIsEnabled = audioManager.musicIsEnabled;
		if (musicIsEnabled)
		{
			musicOn.SetActive(true);
			musicOff.SetActive(false);
		}
		else
		{
			musicOn.SetActive(false);
			musicOff.SetActive(true);
		}
	}
}
