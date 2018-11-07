using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingBackground : MonoBehaviour
{
	public List<Sprite> sectionSprites;

	public int offsetY = 2; // Para tilear la nueva imagen antes de que se vea cuando termina
	public bool hasImageOnBottom = false;
	public bool hasImageOnTop = false;
	public bool reverseScale = false;

	private GameObject background;
	private Camera mainCamera;
	private float backgroundHeight = 0f;
	private float camHeightFromCenter = 0f;

	void Start()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		mainCamera = Camera.main;

		// Distancia desde el punto medio hasta el extremo del sprite
		backgroundHeight = spriteRenderer.sprite.bounds.size.y;
		camHeightFromCenter = mainCamera.orthographicSize;
		background = gameObject;
	}

	void Update ()
	{
		if (mainCamera.transform.position.y + mainCamera.orthographicSize >= transform.position.y + (backgroundHeight / 2) - offsetY && !hasImageOnTop)
		{
			CreateTilingImage();
			hasImageOnTop = true;
		}

		if (mainCamera.transform.position.y - camHeightFromCenter >= transform.position.y + (backgroundHeight / 2) + offsetY)
			Destroy(gameObject);
	}

	void CreateTilingImage()
	{
		float newImageYPos = transform.position.y + backgroundHeight;

		Vector2 newPos = new Vector2(transform.position.x, newImageYPos);
		GameObject newImage = Instantiate(background, newPos, Quaternion.identity);

		// Reverse image to loop
		//newImage.transform.localScale = new Vector2(newImage.transform.localScale.x, newImage.transform.localScale.y * -1);

		newImage.transform.parent = transform.parent;
		newImage.GetComponent<TilingBackground>().hasImageOnBottom = true;
		newImage.GetComponent<SpriteRenderer>().sprite = sectionSprites[LevelManager.GetInstance().CurrentSectionIndex];
	}
}
