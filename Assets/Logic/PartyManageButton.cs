using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PartyManageButton : MonoBehaviour {

	private Image button_image;
	[SerializeField] Text t_Text;
	private Character affected_character;
	private void Awake(){
		button_image = this.gameObject.GetComponent<Image> ();
		affected_character = this.gameObject.transform.parent.gameObject.GetComponent<Portrait> ().character;
	}
	private void OnEnable(){
		UpdateImage ();
	}

	public void OnClick(){
		if(affected_character.inFightingParty)	affected_character.RemoveFromParty();
		else affected_character.AddToParty();
		UpdateImage ();
	}

	public void UpdateImage(){
		t_Text.text = (affected_character.inFightingParty) ? "BATTLE" : "SLEEPING";
	}

}
