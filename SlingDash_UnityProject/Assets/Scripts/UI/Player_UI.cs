using System;
using UnityEngine;

public class Player_UI : MonoBehaviour 
{
	[SerializeField] GameObject pauseMenu;

	public static event Action OnPause_Event;

	public void OnPauseButtonPressed () 
	{
		OnPause_Event();
		pauseMenu.SetActive(true);
	}
}
