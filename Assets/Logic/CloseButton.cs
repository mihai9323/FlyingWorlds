using UnityEngine;
using System.Collections;

public class CloseButton : MonoBehaviour {

	public void CloseParent(){
		string panel = this.gameObject.transform.parent.name;
		switch(panel){
		case "Characters": HubManager.HideCharacters(); break;
		case "Character":  HubManager.HideCharacter(); break;
		case "Inventory":  HubManager.HideInventory(); break;
		case "Shop": HubManager.HideShop(); break;
	
			
		}
	}
}
