using UnityEngine;
using UnityEngine.UI;

public class Boss_UI : MonoBehaviour 
{
	public SheepBoss sheepBoss;
	public Slider healthbar;

	void Start () 
	{
		sheepBoss.LostHealth_Event += UpdateHealthbar;
		sheepBoss.ArrivedToPosition_Event += DisplayHealthbar;
	}
	
	void DisplayHealthbar()
	{
		healthbar.gameObject.SetActive(true);
	}

	void UpdateHealthbar(int health) 
	{
		healthbar.value = health;
	}
}
