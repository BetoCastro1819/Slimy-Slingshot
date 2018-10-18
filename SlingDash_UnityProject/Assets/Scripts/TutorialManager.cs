using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	// Movement tutorial
	public GameObject movementAnim;
	public GameObject movementAnimText;

	// Slow-motion tutorial
	public GameObject slowMoAnim;
	public GameObject slowMoAnimText;

	// Shooting enemies tutorial
	public GameObject shootingAnim;
	public GameObject shootingAnimText;

	// Buttons
	public GameObject prevBtn;
	public GameObject nextBtn;

	private GameObject currentAnim;
	private GameObject currentText;
	private int tutorialIndex;

	private void Start()
	{
		tutorialIndex = 0;
	}

	private void Update()
	{
		PlayTutorialAnimation(tutorialIndex);
	}

	void PlayTutorialAnimation(int index)
	{
		switch (index)
		{
			case 0:
				MovementTutorial();
				break;
			case 1:
				SlowMoTutorial();
				break;
			case 2:
				ShootingTutorial();
				break;
		}
	}

	void MovementTutorial()
	{
		nextBtn.SetActive(true);
		prevBtn.SetActive(false);

		if (movementAnim.activeInHierarchy == false)
		{
			movementAnim.SetActive(true);
			movementAnimText.SetActive(true);

			slowMoAnim.SetActive(false);
			slowMoAnimText.SetActive(false);

			shootingAnim.SetActive(false);
			shootingAnimText.SetActive(false);
		}
	}

	void SlowMoTutorial()
	{
		nextBtn.SetActive(true);
		prevBtn.SetActive(true);

		if (slowMoAnim.activeInHierarchy == false)
		{
			movementAnim.SetActive(false);
			movementAnimText.SetActive(false);

			slowMoAnim.SetActive(true);
			slowMoAnimText.SetActive(true);

			shootingAnim.SetActive(false);
			shootingAnimText.SetActive(false);
		}
	}

	void ShootingTutorial()
	{
		nextBtn.SetActive(false);
		prevBtn.SetActive(true);

		if (shootingAnim.activeInHierarchy == false)
		{
			movementAnim.SetActive(false);
			movementAnimText.SetActive(false);

			slowMoAnim.SetActive(false);
			slowMoAnimText.SetActive(false);

			shootingAnim.SetActive(true);
			shootingAnimText.SetActive(true);
		}
	}

	public void Next()
	{
		tutorialIndex++;
	}

	public void Previous()
	{
		tutorialIndex--;
	}

}
