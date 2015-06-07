using UnityEngine;
using System.Collections;

public class CloseButton : MonoBehaviour {

	[SerializeField] GameObject m_tutorialPanel;

	public void CloseParent(){
		string panel = this.gameObject.transform.parent.name;
		switch(panel){
		case "Characters": HubManager.HideAll(); break;
		case "Character":  HubManager.HideAll(); break;
		case "Inventory":  HubManager.HideAll(); break;
		case "Shop": HubManager.HideAll(); break;
		case "QuestUI": HubManager.HideAll(); break;
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
	public void HideBattleTutorial(){
		m_tutorialPanel.SetActive (false);
		HubManager.road.OnClick ();

	}
	private void Start(){
		if (m_tutorialPanel != null) {
			if (m_tutorialPanel.activeInHierarchy) {
				HubManager.interactable = false;
			}
		}
	}
	private void OnEnable(){
		if(Application.loadedLevel>1){
			if (m_tutorialPanel != null) {
				m_tutorialPanel.SetActive(false);
			}
		}
	}
}
