using UnityEngine;
using System.Collections;

public class ItemOnCharacter : MonoBehaviour {

	public Item currentItem;
	public void OpenInventory(){
		HubManager.ShowInventory();
	}
}
