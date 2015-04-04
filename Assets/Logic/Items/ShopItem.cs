using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShopItem : MonoBehaviour {
	
	public Item itemInField;
	[SerializeField] Image itemImage; 
	private void OnEnable(){
		DisplayItem();
	}
	public void OnClickMe(){
		HubManager.shop.CompareMyItem(this.itemInField);
	}
	public void OnClickShop(){
		HubManager.shop.CompareSaleItem(this.itemInField);
	}
	public void DisplayItem(){
		
		if(itemInField != null && itemInField.itemType != Item.ItemType.None){
			Color outColor;
			itemImage.enabled = true;
			itemImage.sprite = itemInField.GetSprite(out outColor);
			itemImage.color = outColor;
		}else if(itemInField.itemType == Item.ItemType.None){
			itemImage.enabled = false;
		}
	}
	
}
