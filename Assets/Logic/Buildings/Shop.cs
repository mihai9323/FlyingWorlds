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
	[SerializeField] Text SellPrice;

	[SerializeField] Text T_my_item_name;
	[SerializeField] Text T_other_item_name;

	[SerializeField] GameObject BuyButton,SwapButton,SellButton;
	
	private int myItemValue, otherItemValue;

	public Item sellItem, buyItem;
	public int level;
	public void CompareMyItem(Item item){
		sellItem = item;
		if(item.itemType == Item.ItemType.None){
			T_my_dmg.text = "No Item Selected";
			T_my_speed.text = "";
			T_my_shine.text = "";
			myItemValue = 0;

		}
		else if(item.itemType == Item.ItemType.Armor){
			T_my_dmg.text = "DEF: "+item.Defence;
			T_my_speed.text = "";
			T_my_shine.text= "SHINE: "+ item.shine;
			myItemValue = item.Value;
	
			
		}else{
			T_my_dmg.text = "DMG: "+item.Damage;
			T_my_speed.text = "RNG: "+item.Range;
			T_my_shine.text = "SHINE: "+ item.shine;
			myItemValue = item.Value;

		}
		T_my_item_name.text = "SELL \n" + item.ItemName;
		DisplayButtons ();
	}
	public void CompareSaleItem(Item item){
		buyItem = item;
		if(item.itemType == Item.ItemType.None){
			T_other_dmg.text = "No item selected";
			T_other_speed.text = "";
			T_other_shine.text = "";
			otherItemValue = 0;
		
		}
		else if(item.itemType == Item.ItemType.Armor){
			T_other_dmg.text = "DEF: "+item.Defence;
			T_other_speed.text = "";
			T_other_shine.text = "SHINE: "+ item.shine;
			otherItemValue = item.Value;

			
		}else{
			T_other_dmg.text = "DMG: "+item.Damage;
			T_other_speed.text = "RNG: "+item.Range;
			T_other_shine.text = "SHINE: "+ item.shine;
			otherItemValue = item.Value;
		
		}
		T_other_item_name.text = "BUY \n" + item.ItemName;
		DisplayButtons ();
	}

	public void BuyItem(){
		BuyFunctionality ();
		InventoryManager.PopulateInventory ();
		InventoryManager.PopulateShop ();
	}
	public void SellItem(){
		SellFunctionality ();
		InventoryManager.PopulateInventory ();
		InventoryManager.PopulateShop ();

	}
	public void SwapItem(){
		BuyFunctionality ();
		SellFunctionality ();
		InventoryManager.PopulateInventory ();
		InventoryManager.PopulateShop ();
	}
	private void BuyFunctionality(){
		if (GameData.NumberOfCoins > otherItemValue) {
			if(InventoryManager.ItemsForSale.Contains(buyItem)){
				GameData.NumberOfCoins -= otherItemValue;
				InventoryManager.ItemsInInventory.Add(buyItem);
				InventoryManager.ItemsForSale.Remove(buyItem);
				CompareSaleItem(new Item());
			}
		}
	}
	private void SellFunctionality(){
		if(InventoryManager.ItemsInInventory.Contains(sellItem)){
			GameData.NumberOfCoins += myItemValue;
			InventoryManager.ItemsForSale.Add(sellItem);
			InventoryManager.ItemsInInventory.Remove(sellItem);
			CompareMyItem(new Item());
		}
	}

	private void DisplayButtons(){
		if (myItemValue == 0 && otherItemValue == 0) {
			BuyButton.SetActive(false);
			SellButton.SetActive(false);
			SwapButton.SetActive(false);
		}else
		if (myItemValue == 0 && otherItemValue != 0) {

			if(!InventoryManager.InventoryIsFull()){
				BuyButton.SetActive(true);
				BuyPrice.text = "Buy for:"+otherItemValue;
			}
			SellButton.SetActive(false);
			SwapButton.SetActive(false);
		}else
		if (myItemValue != 0 && otherItemValue == 0) {
			BuyButton.SetActive(false);

			if(!InventoryManager.ShopIsFull()){

				SellButton.SetActive(true);
				SellPrice.text = "Sell for:"+myItemValue;
			}
			SwapButton.SetActive(false);
		}else
		if (myItemValue != 0 && otherItemValue != 0) {
			BuyButton.SetActive(false);
			SellButton.SetActive(false);
			SwapButton.SetActive(true);
			SwapPrice.text = "Swap for:"+(otherItemValue - myItemValue).ToString();
		}
	}

	public void ShowShop(){
		HubManager.ShowShop();
	}
}
