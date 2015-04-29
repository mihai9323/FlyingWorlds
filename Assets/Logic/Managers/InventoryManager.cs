using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
	
	private static InventoryManager s_Instance;
	public int startItemCount = 8;
	public int shopItemCount = 12;
	public static List<Item> ItemsInInventory;
	public static List<Item> ItemsForSale;
	public static InventoryItem[] InventoryItems{
		get{
			return s_Instance.inventoryItems;
		}
	}
	
	[SerializeField] InventoryItem[] inventoryItems;
	
	public static ShopItem[] ShopItems{
		get{
			return s_Instance.shopItems;
		}
	}
	
	[SerializeField] ShopItem[] shopItems;
	
	public static ShopItem[] ForSaleItems{
		get{
			return s_Instance.forSaleItems;
		}
	}
	
	[SerializeField] ShopItem[] forSaleItems;
	
	
	private void Awake(){
		ItemsInInventory = GenerateItems(startItemCount,1,10);
		ItemsForSale = GenerateItems(shopItemCount, 5,20);
		s_Instance = this;
				
	}

	public static void GenerateShopItems(int minVal, int maxVal){

	}
	public static void GenerateLootItems(int count, int minVal, int maxVal){

	}

	private void Start(){
		
		PopulateInventory();
	}
	public static bool InventoryIsFull(){
		foreach (InventoryItem item in InventoryItems) {
			if(item.itemInField == null || item.itemInField.itemType == Item.ItemType.None){
				return false;
			}
		}
		return true;
	}
	public static bool ShopIsFull(){
		foreach (ShopItem item in ForSaleItems) {
			if(item.itemInField == null || item.itemInField.itemType == Item.ItemType.None){
				return false;
			}
		}
		return true;
	}

	public static void PopulateInventory(){
		int c = 0;
		for(int i =0; i<InventoryItems.Length; i++){
			Item item = GetNextNotUsedItem(ref c,ItemsInInventory);
			if(item!=null){
				InventoryItems[i].itemInField = item;
				ShopItems[i].itemInField = item;
		
			}else{
				InventoryItems[i].itemInField = new Item();
				ShopItems[i].itemInField = new Item();
			}
			ShopItems[i].DisplayItem();
		}
	}
	public static void PopulateShop(){
		int c = 0;
		for(int i =0; i<ForSaleItems.Length; i++){
			Item item = GetNextNotUsedItem(ref c,ItemsForSale);
			if(item!=null){
				ForSaleItems[i].itemInField = item;
				
				
			}else{
				ForSaleItems[i].itemInField = new Item();
				
			}
			ForSaleItems[i].DisplayItem();
		}
	}
	private static Item GetNextNotUsedItem(ref int c, List<Item> itemArray){
		if(itemArray!=null){
			for(int i = c; i< itemArray.Count; i++){
				if(itemArray[i].itemOwner == null){
					c = i+1;
					return itemArray[i];
				}
			}
		}
		c = itemArray.Count;
		return null;
	}
	public static InventoryItem GetNextEmptyInventorySpot(int lookFrom){
		for (int i = lookFrom; i<InventoryItems.Length; i++) {
			Item item = InventoryItems[i].itemInField;
			if(item == null || (item.itemOwner == null && item.itemType == Item.ItemType.None)){
				return InventoryItems[i];
			}
		}
		return null;
	}


	public static List<Item> GenerateItems(int nr, float minVal, float maxVal){
		List<Item> generatedItems = new List<Item>();
		for(int i =0; i<nr; i++){
			generatedItems.Add(new Item((int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal)));
		}
		return generatedItems;
	}
	
}


