using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_UI : MonoBehaviour 
{
	[SerializeField] AudioClip buttonSound;
	public static event Action OnResumeGame_Event;

	public void OnResumeButtonPressed() 
	{
		PlayButtonSound();
		OnResumeGame_Event();
		gameObject.SetActive(false);
	}
	
	public void OpenScene(string sceneName)
	{
		PlayButtonSound();
		SceneManager.LoadScene(sceneName);
	}

	public void Replay()
	{
		PlayButtonSound();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void PlayButtonSound()
	{
		AudioManager.Instance.PlayAudioClip(buttonSound);
	}
}
