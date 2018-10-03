using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
	public Slider energyBar;
    public Text meterText;
    public Text scoreText;

	#region Singleton
	private static UI_Manager instance;
	public static UI_Manager Get()
	{
		return instance;
	}
	private void Awake()
	{
		instance = this;
	}
	#endregion
}
