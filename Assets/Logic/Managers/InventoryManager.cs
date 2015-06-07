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
	
	public static ShopItem[] myItemsInShop{
		get{
			return s_Instance._myItemsInShop;
		}
	}
	
	[SerializeField] ShopItem[] _myItemsInShop;
	
	public static ShopItem[] shopKeeperItems{
		get{
			return s_Instance._shopsKeeperItems;
		}
	}
	
	[SerializeField] ShopItem[] _shopsKeeperItems;
	
	
	private void Awake(){
		//ItemsInInventory = GenerateItems(startItemCount,1,10);
		ItemsInInventory = GenerateArmor (4, 1, 6);
		ItemsInInventory.AddRange(GenerateWeapons(4,1,8));
		ItemsForSale = GenerateItems(shopItemCount, 5,20);
		s_Instance = this;
				
	}

	public static void GenerateShopItems(int count, int minVal, int maxVal){
		ItemsForSale = GenerateItems (count, minVal, maxVal);
		PopulateShop ();
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
		foreach (ShopItem item in shopKeeperItems) {
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
				myItemsInShop[i].itemInField = item;
		
			}else{
				InventoryItems[i].itemInField = new Item();
				myItemsInShop[i].itemInField = new Item();
			}
			myItemsInShop[i].DisplayItem();
		}
	}
	public static void PopulateShop(){
		int c = 0;
		for(int i =0; i<shopKeeperItems.Length; i++){
			Item item = GetNextNotUsedItem(ref c,ItemsForSale);
			if(item!=null){
				shopKeeperItems[i].itemInField = item;
				
				
			}else{
				shopKeeperItems[i].itemInField = new Item();
				
			}
			shopKeeperItems[i].DisplayItem();
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
	public static List<Item> GenerateArmor(int nr, float minVal, float maxVal){
		List<Item> generatedItems = new List<Item>();
		for(int i =0; i<nr; i++){
			generatedItems.Add(new Item(Item.ItemType.Armor,(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal)));
		}
		return generatedItems;
	}
	public static List<Item> GenerateWeapons(int nr, float minVal, float maxVal){
		List<Item> generatedItems = new List<Item>();
		for(int i =0; i<nr; i++){
			float r = Random.value;
			if(r<.3f)generatedItems.Add(new Item(Item.ItemType.Magic,(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal)));
			else if(r<.6f) generatedItems.Add(new Item(Item.ItemType.Melee,(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal)));
			else generatedItems.Add(new Item(Item.ItemType.Ranged,(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal)));
			
		}
		return generatedItems;
	}
	
}


