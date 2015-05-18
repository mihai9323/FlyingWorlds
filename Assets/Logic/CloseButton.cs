using UnityEngine;
using System.Collections;

public class CloseButton : MonoBehaviour {

	[SerializeField] GameObject m_tutorialPanel;

	public void CloseParent(){
		string panel = this.gameObject.transform.parent.name;
		switch(panel){
		case "Characters": HubManager.HideCharacters(); break;
		case "Character":  HubManager.HideCharacter(); break;
		case "Inventory":  HubManager.HideInventory(); break;
		case "Shop": HubManager.HideShop(); break;
		case "QuestUI": HubManager.HideQuestUI(); break;
		}
	}
	public void CloseAll(){
		HubManager.HideAll ();
	}
	public void ToggleTutorial(){
		m_tutorialPanel.SetActive (!m_tutorialPanel.activeInHierarchy);
		if (m_tutorialPanel.activeInHierarchy) {
			HubManager.interactable = false;
		} else
			HubManager.interactable = true;
	}
	private void Start(){
		if (m_tutorialPanel.activeInHierarchy) {
			HubManager.interactable = false;
		}
	}
}
