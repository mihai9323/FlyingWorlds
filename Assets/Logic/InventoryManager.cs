using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
	
	private static InventoryManager s_Instance;
	public int startItemCount = 8;
	public int shopItemCount = 12;
	public static Item[] Items;
	public static Item[] ItemsForSale;
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
		Items = GenerateItems(startItemCount,1,10);
		ItemsForSale = GenerateItems(shopItemCount, 5,20);
		s_Instance = this;
				
	}
	private void Start(){
		
		PopulateInventory();
	}
	
	public static void PopulateInventory(){
		int c = 0;
		for(int i =0; i<InventoryItems.Length; i++){
			Item item = GetNextNotUsedItem(ref c,Items);
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
	private static Item GetNextNotUsedItem(ref int c, Item[] itemArray){
		if(itemArray!=null){
			for(int i = c; i< itemArray.Length; i++){
				if(itemArray[i].itemOwner == null){
					c = i+1;
					return itemArray[i];
				}
			}
		}
		c = itemArray.Length;
		return null;
	}
	public static Item[] GenerateItems(int nr, float minVal, float maxVal){
		Item[] generatedItems = new Item[nr];
		for(int i =0; i<generatedItems.Length; i++){
			generatedItems[i] = new Item((int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal),(int)Random.Range(minVal,maxVal));
		}
		return generatedItems;
	}
	
}


