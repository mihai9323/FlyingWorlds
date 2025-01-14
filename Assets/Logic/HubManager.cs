﻿using UnityEngine;
using System.Collections;

public class HubManager : MonoBehaviour {

	
	
	private static HubManager s_Instance;
	
	[SerializeField] GameObject m_Characters, m_Character, m_Inventory,m_Shop;
	[SerializeField] Farm m_farm;
	[SerializeField] Mine m_mine;
	[SerializeField] Shop m_shop;
	
	public static Farm farm{get{return s_Instance.m_farm;}}
	public static Mine mine{get{return s_Instance.m_mine;}}
	public static Shop shop{get{return s_Instance.m_shop;}}
	
	
	
	private void Awake(){
		s_Instance = this;
		CharacterManager.SelectedCharacter = null;
	}
	private void Start(){
		m_Character.SetActive(false);
		m_Characters.SetActive(false);
		m_Inventory.SetActive(false);
		m_Shop.SetActive(false);
	}
	
	
	
	
	//Show functions
	public static void ShowCharacters(){
		s_Instance.m_Characters.SetActive(true);
		CharacterManager.SelectedCharacter = null;
	}
	public static void ShowCharacter(Character character){
		s_Instance.m_Character.SetActive(true);
		s_Instance.m_Character.GetComponent<CharacterPanel>().SetInfo(character);
		CharacterManager.SelectedCharacter = character;
		
	}
	public static void ShowInventory(){
		InventoryManager.PopulateInventory();
		s_Instance.m_Inventory.SetActive(true);
	}
	public static void ShowShop(){
		InventoryManager.PopulateInventory();
		InventoryManager.PopulateShop();
		s_Instance.m_Shop.SetActive(true);
		CharacterManager.SelectedCharacter = null;
		
	}
	//Hide functions
	public static void HideCharacters(){
		s_Instance.m_Characters.SetActive(false);
		HideCharacter();
		
	}
	public static void HideCharacter(){
		s_Instance.m_Character.SetActive(false);
		CharacterManager.SelectedCharacter = null;
		HideInventory();
	}
	public static void HideInventory(){
		s_Instance.m_Inventory.SetActive(false);
	}
	public static void HideShop(){
		s_Instance.m_Shop.SetActive(false);
	}
}
