﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class InventoryItem : MonoBehaviour {

	[SerializeField] Image img_item;
	[SerializeField] Image img_owner;
	
	public Item itemInField;
	
	public int buttonId;
	
	private void Start(){
		DisplayItem();
	}
	
	
	public void DisplayItem(){
		if(itemInField != null && itemInField.itemType != Item.ItemType.None){
			Color outColor;
			img_item.enabled = true;
			img_item.sprite = itemInField.GetSprite(out outColor);
			img_item.color = outColor;
		}else if(itemInField.itemType == Item.ItemType.None){
			img_item.enabled = false;
		}
	}
	public void SelectItem(){
		
		if(itemInField.itemType == Item.ItemType.Armor){
			SwapItem (CharacterManager.SelectedCharacter.ArmorItem);
		}else if(itemInField.itemType != Item.ItemType.None){
			SwapItem(CharacterManager.SelectedCharacter.WeaponItem, false);
		}else if(CharacterManager.SelectedCharacter.WeaponItem.itemType != null){
			SwapItem(CharacterManager.SelectedCharacter.WeaponItem, false);
		}else if(CharacterManager.SelectedCharacter.ArmorItem !=null){
			SwapItem(CharacterManager.SelectedCharacter.ArmorItem);
		}
	}

	void SwapItem (Item charactersItem, bool armor = true)
	{
		Item auxItem = itemInField;
		auxItem.itemOwner = CharacterManager.SelectedCharacter;
		itemInField = charactersItem;
		itemInField.itemOwner = null;
		if(armor)CharacterManager.SelectedCharacter.ArmorItem = auxItem;
		else CharacterManager.SelectedCharacter.WeaponItem = auxItem;
		
		DisplayItem ();
		HubManager.ShowCharacter (CharacterManager.SelectedCharacter);
		
	}
}
