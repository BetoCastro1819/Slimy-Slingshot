using UnityEngine;

public class SfxToggle : MonoBehaviour 
{
	[SerializeField] GameObject sfxOn;
	[SerializeField] GameObject sfxOff;

	AudioManager audioManager;

	private void Start()
	{
		audioManager = AudioManager.Instance;
		UpdateButtonUI();
	}

	public void ToggleSfx()
	{
		audioManager.ToggleSfx();
		UpdateButtonUI();
	}

	private void UpdateButtonUI()
	{
		bool sfxAreEnabled = audioManager.sfxAreEnabled;
		if (sfxAreEnabled)
		{
			sfxOn.SetActive(true);
			sfxOff.SetActive(false);
		}
		else
		{
			sfxOn.SetActive(false);
			sfxOff.SetActive(true);
		}
	}
}
