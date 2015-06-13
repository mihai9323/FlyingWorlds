using UnityEngine;
using System.Collections;

public class HubManager : MonoBehaviour {

	
	
	private static HubManager s_Instance;
	
	[SerializeField] GameObject m_Characters, m_Character, m_Inventory,m_Shop;
	[SerializeField] Farm m_farm;
	[SerializeField] QuestUI m_questUI;

	[SerializeField] Shop m_shop;
	[SerializeField] Road m_road;
	[SerializeField] NotificationBox m_notif;

	public static Farm farm{get{return s_Instance.m_farm;}}

	public static Shop shop{get{return s_Instance.m_shop;}}
	public static Road road{ get { return s_Instance.m_road; } }
	public static QuestUI questUI{ get { return s_Instance.m_questUI; } }

	public static NotificationBox notification{ get { return s_Instance.m_notif; } }
	public static bool interactable = true;
	public static bool hintsOn = false;
	public static bool noPanelOpened{
		get{
			bool rValue =  !shop.gameObject.activeInHierarchy && 				   
					!notification.gameObject.activeInHierarchy && 
					!s_Instance.m_Characters.gameObject.activeInHierarchy &&
					!s_Instance.m_Character.gameObject.activeInHierarchy &&
					!s_Instance.m_Inventory.gameObject.activeInHierarchy &&
					!s_Instance.m_Shop.gameObject.activeInHierarchy ;

			if(questUI!= null) return rValue && !questUI.gameObject.activeInHierarchy; 

			return rValue;  

			
		}
	}
	
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
	
	private void OnEnable(){
		ShowQuestInfo ();
	}
	
	
	//Show functions
	public static void ShowCharacters(){
		if (interactable) {
			HideAll();

			s_Instance.m_Characters.SetActive (true);
			CharacterManager.SelectedCharacter = null;
			ShowCharacter(CharacterManager.gameCharacters[0]);
			ShowInventory();
		}
	}
	//Show functions
	public static void ShowCharacters(Character c){
		if (interactable) {
			HideAll();

			s_Instance.m_Characters.SetActive (true);
			CharacterManager.SelectedCharacter = c;
			ShowCharacter(c);
			ShowInventory();
		}
	}
	public static void ShowCharacter(Character character){
		if (interactable) {

			s_Instance.m_Character.SetActive (true);

			CharacterManager.SelectedCharacter = character;
			s_Instance.m_Character.GetComponent<CharacterPanel> ().SetInfo (character);
		}
		
	}
	public static void ShowInventory(){
		if (interactable) {

			InventoryManager.PopulateInventory ();
			s_Instance.m_Inventory.SetActive (true);
		}
	}
	public static void ShowShop(){
		if (interactable) {
			HideAll();

			InventoryManager.PopulateInventory ();
			InventoryManager.PopulateShop ();
			s_Instance.m_Shop.SetActive (true);
			CharacterManager.SelectedCharacter = null;
		}
	}
	public static void ShowQuestInfo(){
		HideAll ();

		if (interactable) {

			if(s_Instance.m_questUI!=null)s_Instance.m_questUI.gameObject.SetActive(true);


		}
	}

	//Hide functions
	public static void HideCharacters(){
		if (interactable) {

			s_Instance.m_Characters.SetActive (false);
			HideCharacter ();
		}
		
	}
	public static void HideCharacter(){
		if (interactable) {
			s_Instance.m_Character.SetActive (false);
			CharacterManager.SelectedCharacter = null;
			HideInventory ();
		}
	}
	public static void HideInventory(){
		if (interactable) {
			s_Instance.m_Inventory.SetActive (false);
		}
	}
	public static void HideShop(){
		if (interactable) {
			s_Instance.m_Shop.SetActive (false);
		}
	}
	public static void HideQuestUI(){
		if (interactable) {
			if(s_Instance.m_questUI!=null)s_Instance.m_questUI.gameObject.SetActive(false);

		}
	}
	                             
	public static void HideAll(){
		HideQuestUI ();
		HideShop ();
		HideCharacters ();

	}
	//Checks
	public static bool InventoryOpen{
		get{
			return s_Instance.m_Inventory.activeInHierarchy;
		}
	}

	private void Update(){
		if (road.CharactersTravellingToFightScene) {
//			Debug.Log("traveling");
			if(Input.GetMouseButtonDown(0)){
				
				foreach(Character c in CharacterManager.gameCharacters){
					if(c.inFightingParty){
						c.MoveCharacterToPosition(c.transform.position,delegate(Character character) {

						});
					}
				}
				road.CharactersTravellingToFightScene = false;
				HubManager.interactable = true;
			}
		}

	}

}
