using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton_UI : MonoBehaviour 
{

	public InventoryItemsParent_UI inventoryParent;

	[SerializeField] AudioClip buttonSound;
	[SerializeField] GameObject itemEquippedMessage;
	[SerializeField] string itemPath;

	private Button button;

	void Start()
	{
		button = GetComponent<Button>();
		UpdateItemEquippedState();
	}

	public void EquipItem()
	{
		AudioManager.Instance.PlayAudioClip(buttonSound);
		PersistentGameData.Instance.SetTrailAsCurrent(itemPath);
		UpdateItemEquippedState();
		inventoryParent.UpdateItemsState();
	}

	public void UpdateItemEquippedState()
	{
		if (!button)
			button = GetComponent<Button>();

		bool itemIsEquipped = (PersistentGameData.Instance.gameData.currentPlayerTrail == itemPath);
		if (itemIsEquipped)
		{
			button.interactable = false;
			itemEquippedMessage.SetActive(true);
		}
		else
		{
			button.interactable = true;
			itemEquippedMessage.SetActive(false);
		}
	}
}
