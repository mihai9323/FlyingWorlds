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
		if(item==null){
			item = new Item();
		}
		currentItem = item;
		Color outColor;
		m_itemSprite.sprite = currentItem.GetSprite(out outColor);
		m_itemSprite.color = outColor;
	}
	
	
	
}
