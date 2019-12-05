using System;
using UnityEngine;

public class PauseMenu_UI : MonoBehaviour 
{
	public static event Action OnResumeGame_Event;

	public void OnResumeButtonPressed() 
	{
		OnResumeGame_Event();
		gameObject.SetActive(false);
	}
	
	public void OnBackToMenu()
	{
	 
	}

	public void OnOpenStore()
	{

	}
}
