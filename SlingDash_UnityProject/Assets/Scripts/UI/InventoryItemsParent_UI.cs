using System.Collections.Generic;
using UnityEngine;

public class InventoryItemsParent_UI : MonoBehaviour 
{
	public List<InventoryItemButton_UI> inventoryItems;

	public void UpdateItemsState()
	{
		for (int i = 0; i < inventoryItems.Count; i++)
		{
			inventoryItems[i].UpdateItemEquippedState();
		}
	}
}
