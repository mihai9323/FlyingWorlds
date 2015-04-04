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
	
	[SerializeField] Text BuyPrice;
	[SerializeField] Text SwapPrice;
	
	private float myItemValue, otherItemValue;
	public Item sellItem, buyItem;
	public void CompareMyItem(Item item){
		sellItem = item;
		if(item.itemType == Item.ItemType.None){
			T_my_dmg.text = "DMG: ";
			T_my_speed.text = "SPEED: ";
			T_my_shine.text = "SHINE: ";
			myItemValue = 0;
			BuyPrice.text = "Buy for "+otherItemValue;
			SwapPrice.text = "Swap for "+(otherItemValue- myItemValue).ToString();
		}
		else if(item.itemType == Item.ItemType.Armor){
			T_my_dmg.text = "DEF: "+item.Defence;
			T_my_speed.text = "";
			T_my_shine.text= "SHINE: "+ item.shine;
			myItemValue = item.Value;
			BuyPrice.text = "Buy for "+otherItemValue;
			SwapPrice.text = "Swap for "+(otherItemValue- myItemValue).ToString();
			
		}else{
			T_my_dmg.text = "DMG: "+item.Damage;
			T_my_speed.text = "SPD: "+item.Speed;
			T_my_shine.text = "SHINE: "+ item.shine;
			myItemValue = item.Value;
			BuyPrice.text = "Buy for "+otherItemValue;
			SwapPrice.text = "Swap for "+(otherItemValue- myItemValue).ToString();
		}
	}
	public void CompareSaleItem(Item item){
		buyItem = item;
		if(item.itemType == Item.ItemType.None){
			T_other_dmg.text = "DMG: ";
			T_other_speed.text = "SPEED: ";
			T_other_shine.text = "SHINE: ";
			otherItemValue = 0;
			BuyPrice.text = "Buy for "+otherItemValue;
			SwapPrice.text = "Swap for "+otherItemValue;
			
		}
		else if(item.itemType == Item.ItemType.Armor){
			T_other_dmg.text = "DEF: "+item.Defence;
			T_other_speed.text = "";
			T_other_shine.text = "SHINE: "+ item.shine;
			otherItemValue = item.Value;
			BuyPrice.text = "Buy for "+otherItemValue;
			SwapPrice.text = "Swap for "+(otherItemValue- myItemValue).ToString();
			
		}else{
			T_other_dmg.text = "DMG: "+item.Damage;
			T_other_speed.text = "SPD: "+item.Speed;
			T_other_shine.text = "SHINE: "+ item.shine;
			otherItemValue = item.Value;
			BuyPrice.text = "Buy for "+otherItemValue;
			SwapPrice.text = "Swap for "+(otherItemValue- myItemValue).ToString();
		}
	}
	public void ShowShop(){
		HubManager.ShowShop();
	}
}
