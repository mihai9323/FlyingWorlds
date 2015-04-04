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
		HubManager.ShowInventory();
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
