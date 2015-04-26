using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemOnCharacter : MonoBehaviour {
	
	[SerializeField] Image m_itemSprite;
	
	public enum TypeOfField{
		armor,
		weapon
	}
	public TypeOfField allowedItem;
	
	public Item currentItem;
	public void OpenInventory(){
		if (!HubManager.InventoryOpen)
			HubManager.ShowInventory ();
		else if (!InventoryManager.InventoryIsFull ()) {
			InventoryManager.GetNextEmptyInventorySpot(0).SwapItem(this.currentItem, allowedItem == TypeOfField.armor);
		} else {
			Debug.Log("Inventory full");
		}
	}
	
	public void DisplayItem(Item item){
		Color outColor;
		Sprite sprite;
		if (item == null || item.itemType == Item.ItemType.None) {
			item = new Item ();
			outColor = new Color(0,0,0,0);

		} else {
			sprite = item.GetSprite(out outColor);
			m_itemSprite.sprite = sprite;
		}
		m_itemSprite.color = outColor;
		currentItem = item;
	}
	
	
	
}
