using UnityEngine;
using UnityEngine.UI;

public class StoreItemButton_UI : MonoBehaviour 
{
	[Header("Audio")]
	[SerializeField] AudioClip buttonSound;

	[Header("Inventory items variables")]
	[SerializeField] GameObject invetoryItemsParent;
	[SerializeField] GameObject invetoryItemButtonPrefab;

	[Header("Item variables")]
	[SerializeField] string itemPath;
	[SerializeField] int price;

	[Header("UI elements")]
	[SerializeField] Text priceText;
	[SerializeField] Animator notenoughCoinsPopUp;

	void Start()
	{
		priceText.text = price.ToString() + " x";

		if (PersistentGameData.Instance.gameData.trailsPurchased.Contains(itemPath))
		{
			AddItemToInventory();
			Destroy(gameObject);
		}
	}

	public void PurchaseItem()
	{
		int currentPlayerCoins = PersistentGameData.Instance.gameData.coins; 
		if (currentPlayerCoins >= price)
		{
			AudioManager.Instance.PlayAudioClip(buttonSound);
			PersistentGameData.Instance.SubstractFromCoins(price);
			PersistentGameData.Instance.SetTrailAsCurrent(itemPath);
			PersistentGameData.Instance.AddTrailToPurchaseList(itemPath);

			AddItemToInventory();
		
			Destroy(gameObject);
		}
		else
		{
			notenoughCoinsPopUp.SetTrigger("DisplayPopUp");
		}
	}

	private void AddItemToInventory()
	{
		GameObject inventoryItem = Instantiate(invetoryItemButtonPrefab);
		inventoryItem.name = invetoryItemButtonPrefab.name;
		inventoryItem.transform.parent = invetoryItemsParent.transform;
		inventoryItem.transform.localScale = Vector3.one;

		InventoryItemsParent_UI inventoryParent = invetoryItemsParent.GetComponent<InventoryItemsParent_UI>();
		InventoryItemButton_UI inventoryItemButton = inventoryItem.GetComponent<InventoryItemButton_UI>();
		inventoryItemButton.inventoryParent = inventoryParent;
		inventoryParent.inventoryItems.Add(inventoryItemButton);
		inventoryParent.UpdateItemsState();
		
	}
}
