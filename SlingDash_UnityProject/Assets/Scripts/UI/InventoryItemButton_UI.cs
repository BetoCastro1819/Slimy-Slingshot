using UnityEngine;

public class InventoryItemButton_UI : MonoBehaviour 
{
	public InventoryItemsParent_UI inventoryParent;

	[SerializeField] GameObject itemEquippedMessage;
	[SerializeField] string itemPath;

	void Start()
	{
		UpdateItemEquippedState();
	}

	public void EquipItem()
	{
		PersistentGameData.Instance.SetTrailAsCurrent(itemPath);
		UpdateItemEquippedState();
		inventoryParent.UpdateItemsState();
	}

	public void UpdateItemEquippedState()
	{
		bool itemIsEquipped = (PersistentGameData.Instance.gameData.currentPlayerTrail == itemPath);
		if (itemIsEquipped)
			itemEquippedMessage.SetActive(true);
		else
			itemEquippedMessage.SetActive(false);
	}
}
