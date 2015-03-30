using UnityEngine;
using System.Collections;

public class HubManager : MonoBehaviour {

	
	
	private static HubManager s_Instance;
	
	[SerializeField] GameObject m_Characters, m_Character, m_Inventory,m_Shop;
	[SerializeField] Farm m_farm;
	[SerializeField] Mine m_mine;
	
	public static Farm farm{get{return s_Instance.m_farm;}}
	public static Mine mine{get{return s_Instance.m_mine;}}
	
	private void Awake(){
		s_Instance = this;
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
	}
	public static void ShowCharacter(Character character){
		s_Instance.m_Character.SetActive(true);
		s_Instance.m_Character.GetComponent<CharacterPanel>().SetInfo(character);
	}
	public static void ShowInventory(){
		s_Instance.m_Inventory.SetActive(true);
	}
	public static void ShowShop(){
		s_Instance.m_Shop.SetActive(true);
	}
	//Hide functions
	public static void HideCharacters(){
		s_Instance.m_Characters.SetActive(false);
		HideCharacter();
		
	}
	public static void HideCharacter(){
		s_Instance.m_Character.SetActive(false);
		HideInventory();
	}
	public static void HideInventory(){
		s_Instance.m_Inventory.SetActive(false);
	}
	public static void HideShop(){
		s_Instance.m_Shop.SetActive(false);
	}
}
