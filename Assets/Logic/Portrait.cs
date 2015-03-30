using UnityEngine;
using System.Collections;

public class Portrait : MonoBehaviour {
	
	[SerializeField] Character character;
	
	public void OpenPortrait(){
		HubManager.ShowCharacter(character);
	}
}
