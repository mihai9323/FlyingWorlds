using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Shop : MonoBehaviour {
	[SerializeField] Text T_my_dmg;
	[SerializeField] Text t_my_range;

	
	[SerializeField] Text T_other_dmg;
	[SerializeField] Text T_other_range;


	[SerializeField] Text T_myItemValue;
	[SerializeField] Text T_buyItemValue;


	[SerializeField] Text T_buyButton;
	[SerializeField] Text T_swapButton;
	[SerializeField] Text T_sellButton;

	[SerializeField] Text T_my_item_name;
	[SerializeField] Text T_other_item_name;
	[SerializeField] Text T_ShopName;
	[SerializeField] Text T_ShopUpgrade;
	[SerializeField] GameObject BuyButton,SwapButton,SellButton;
	[SerializeField] GameObject Empty;
	private int myItemValue, otherItemValue;

	public Item sellItem, buyItem;
	public int level = 1;
	public int upgradeCost{
		get{
			return (int)((level + 1) * 30 * Mathf.Pow(2,level+1));
		}
	}
	public void OnEnable(){
		T_ShopName.text = "SHOP Level " + level;
		T_ShopUpgrade.text = "Upgrade For " + upgradeCost;

		T_my_dmg.text = "";
		t_my_range.text = "";
		T_myItemValue.text = "";
		myItemValue = 0;

		T_other_dmg.text = "";
		T_other_range.text = "";
		T_buyItemValue.text = "";
		otherItemValue = 0;
	}
	public void CompareMyItem(Item item){
		sellItem = item;
		if(item.itemType == Item.ItemType.None){
			T_my_dmg.text = "";
			t_my_range.text = "";
			T_myItemValue.text = "";
			myItemValue = 0;

		}
		else if(item.itemType == Item.ItemType.Armor){
			T_my_dmg.text = "DEF: "+item.Defence;
			t_my_range.text = "";
			T_myItemValue.text = "VALUE: " + item.Value;
			myItemValue = item.Value;
	
			
		}else{
			T_my_dmg.text = "DMG: "+item.Damage;
			t_my_range.text = "RNG: "+item.Range;
			T_myItemValue.text = "VALUE: " + item.Value;
			myItemValue = item.Value;

		}
		T_my_item_name.text = item.ItemName;

		DisplayButtons ();
	}
	public void CompareSaleItem(Item item){
		buyItem = item;
		if(item.itemType == Item.ItemType.None){
			T_other_dmg.text = "";
			T_other_range.text = "";
			T_buyItemValue.text = "";
			otherItemValue = 0;
		
		}
		else if(item.itemType == Item.ItemType.Armor){
			T_other_dmg.text = "DEF: "+item.Defence;
			T_other_range.text = "";
			T_buyItemValue.text = "VALUE: " + item.Value;
			otherItemValue = item.Value;

			
		}else{
			T_other_dmg.text = "DMG: "+item.Damage;
			T_other_range.text = "RNG: "+item.Range;
			T_buyItemValue.text = "VALUE: " + item.Value;
			otherItemValue = item.Value;
		
		}
		T_other_item_name.text = item.ItemName;
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

		if (InventoryManager.ItemsForSale.Contains (buyItem)) {
			if (GameData.Pay (otherItemValue)) {
				InventoryManager.ItemsInInventory.Add (buyItem);
				InventoryManager.ItemsForSale.Remove (buyItem);
				CompareSaleItem (new Item ());
			}else {
				HubManager.notification.ShowNotification("You do not have enough coins to buy the "+buyItem.ItemName+"\n Come back when you have enough!","Ok!",null);
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
				T_buyButton.text = "Buy for:"+otherItemValue;
			}
			SellButton.SetActive(false);
			SwapButton.SetActive(false);
		}else
		if (myItemValue != 0 && otherItemValue == 0) {
			BuyButton.SetActive(false);

			if(!InventoryManager.ShopIsFull()){

				SellButton.SetActive(true);
				T_sellButton.text = "Sell for:"+myItemValue;
			}
			SwapButton.SetActive(false);
		}else
		if (myItemValue != 0 && otherItemValue != 0) {
			BuyButton.SetActive(false);
			SellButton.SetActive(false);
			SwapButton.SetActive(true);
			T_swapButton.text = "Swap for:"+(otherItemValue - myItemValue).ToString();
		}
		Empty.SetActive(!BuyButton.activeInHierarchy && !SellButton.activeInHierarchy && !SwapButton.activeInHierarchy);
	}
	public void UpgradeShop(){
		if (GameData.HasCoins (upgradeCost)) {
			HubManager.notification.ShowConfirm("Are you sure you want to pay "+upgradeCost+" coins for this upgrade?","Sure!","I am not!",delegate() {
				if(GameData.Pay(upgradeCost)){
					level++;
					InventoryManager.GenerateShopItems (12, HubManager.shop.level * 5, HubManager.shop.level * 10);
					CharacterManager.CheckLabels(LabelManager.checkAfterBuildingUpdate);
				}
			});
		} else {
			HubManager.notification.ShowNotification("You dont have enough gold for this upgrade! \n Come back when you have enough!","cancel");
		}
	}
	public void ShowShop(){
		HubManager.ShowShop();
	}
}
