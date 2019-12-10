using UnityEngine;
using UnityEngine.UI;

public class StoreItemButton_UI : MonoBehaviour 
{
	[Header("Inventory items variables")]
	[SerializeField] GameObject invetoryItemsParent;
	[SerializeField] GameObject invetoryItemButtonPrefab;

	[Header("Item variables")]
	[SerializeField] string itemPath;
	[SerializeField] int price;

	[Header("UI elements")]
	[SerializeField] Text priceText;

	void Start()
	{
		// Check against persisten data to see if player already owns this product
		// If he owns it, disable the button and display message

		priceText.text = price.ToString() + " x";
	}

	public void PurchaseItem()
	{
		PersistentGameData.Instance.SubstractFromCoins(price);
		PersistentGameData.Instance.SetTrailAsCurrent(itemPath);

		GameObject inventoryItem = Instantiate(invetoryItemButtonPrefab);
		inventoryItem.name = invetoryItemButtonPrefab.name;
		inventoryItem.transform.parent = invetoryItemsParent.transform;
		inventoryItem.transform.localScale = Vector3.one;

		Destroy(gameObject);

		// Set item as owned
		// Add item to player inventory list
	}
}
