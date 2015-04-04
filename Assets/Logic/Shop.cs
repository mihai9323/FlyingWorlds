using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Shop : MonoBehaviour {
	[SerializeField] Text T_my_dmg;
	[SerializeField] Text T_my_speed;
	[SerializeField] Text T_my_shine;
	
	[SerializeField] Text T_other_dmg;
	[SerializeField] Text T_other_speed;
	[SerializeField] Text T_other_shine;
	
	[SerializeField] Text T_Buy;
	[SerializeField] Text T_Swap;
	[SerializeField] Text T_Sell;
	
	[SerializeField] Button buyButton;
	[SerializeField] Button swapButton;
	[SerializeField] Button sellButton;
	
	private float myItemValue, otherItemValue;
	
	private Item _buyItem,_sellItem;
	
	public Item sellItem{
		set{
			_sellItem = value;
			DisplayButtons();
			
			}
		get{
			if(_sellItem == null) _sellItem = new Item();
			return _sellItem;
		    }
		}
	public Item buyItem{
		set{
			_buyItem = value;
			DisplayButtons();
			
		}
		get{
			if(_buyItem == null) _buyItem = new Item();
			return _buyItem;
		}
	}
	private void Start(){
		buyButton.gameObject.SetActive(false);
		sellButton.gameObject.SetActive(false);
		swapButton.gameObject.SetActive(false);
	}
public void CompareMyItem(Item item){
		
		if(item.itemType == Item.ItemType.None){
			T_my_dmg.text = "DMG: ";
			T_my_speed.text = "SPEED: ";
			T_my_shine.text = "SHINE: ";
			myItemValue = 0;
			
		}
		else if(item.itemType == Item.ItemType.Armor){
			T_my_dmg.text = "DEF: "+item.Defence;
			T_my_speed.text = "";
			T_my_shine.text= "SHINE: "+ item.shine;
			myItemValue = item.Value;
			
			
		}else{
			T_my_dmg.text = "DMG: "+item.Damage;
			T_my_speed.text = "SPD: "+item.Speed;
			T_my_shine.text = "SHINE: "+ item.shine;
			myItemValue = item.Value;
			
		}
		sellItem = item;
	}
	public void CompareSaleItem(Item item){
		
		if(item.itemType == Item.ItemType.None){
			T_other_dmg.text = "DMG: ";
			T_other_speed.text = "SPEED: ";
			T_other_shine.text = "SHINE: ";
			otherItemValue = 0;
			
			
		}
		else if(item.itemType == Item.ItemType.Armor){
			T_other_dmg.text = "DEF: "+item.Defence;
			T_other_speed.text = "";
			T_other_shine.text = "SHINE: "+ item.shine;
			otherItemValue = item.Value;
		
			
		}else{
			T_other_dmg.text = "DMG: "+item.Damage;
			T_other_speed.text = "SPD: "+item.Speed;
			T_other_shine.text = "SHINE: "+ item.shine;
			otherItemValue = item.Value;
			
		}
		buyItem = item;
	}
	private void DisplayButtons(){
		if(buyItem.itemType == Item.ItemType.None && sellItem.itemType == Item.ItemType.None){
			//we have no items selected
			buyButton.gameObject.SetActive(false);
			swapButton.gameObject.SetActive(false);
			sellButton.gameObject.SetActive(false);
		}else if(buyItem.itemType != Item.ItemType.None && sellItem.itemType != Item.ItemType.None){
			//we have both items
			buyButton.gameObject.SetActive(false);
			swapButton.gameObject.SetActive(true);
			sellButton.gameObject.SetActive(false);
			
			T_Swap.text = "Swap For: "+ Mathf.Max (10,otherItemValue - myItemValue);
		
		}else if(buyItem.itemType != Item.ItemType.None && sellItem.itemType == Item.ItemType.None){
			// we only have the shop item
			buyButton.gameObject.SetActive(true);
			swapButton.gameObject.SetActive(false);
			sellButton.gameObject.SetActive(false);
			
			T_Buy.text = "Buy For: "+ Mathf.Max (10,otherItemValue);
			
			
		}else if(buyItem.itemType == Item.ItemType.None && sellItem.itemType != Item.ItemType.None){
			
			//only our item selected
			buyButton.gameObject.SetActive(false);
			swapButton.gameObject.SetActive(false);
			sellButton.gameObject.SetActive(true);
			
			T_Sell.text = "Sell For: "+ Mathf.Max (10,myItemValue);
			
			
		}
		
		
		
	}
	public void ShowShop(){
		HubManager.ShowShop();
	}
}
