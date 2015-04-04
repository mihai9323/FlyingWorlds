using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
	
	private static InventoryManager s_Instance;
	public int startItemCount = 8;
	public static Item[] Items;
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
	
	private void Awake(){
		s_Instance = this;
	}
	private void Start(){
		GenerateItems(startItemCount);
		PopulateInventory();
	}
	
	public static void PopulateInventory(){
		int c = 0;
		for(int i =0; i<InventoryItems.Length; i++){
			Item item = GetNextNotUsedItem(ref c);
			if(item!=null){
				InventoryItems[i].itemInField = item;
				ShopItems[i].itemInField = item;
			}
		}
	}
	
	private static Item GetNextNotUsedItem(ref int c){
		if(Items!=null){
			for(int i = c; i< Items.Length; i++){
				if(Items[i].itemOwner == null){
					c = i+1;
					return Items[i];
				}
			}
		}
		c = Items.Length;
		return null;
	}
	private static void GenerateItems(int nr){
		Items = new Item[nr];
		for(int i =0; i<Items.Length; i++){
			Items[i] = new Item(Random.Range(1,10),Random.Range(1,10),Random.Range(1,10));
		}
	}
	
}


