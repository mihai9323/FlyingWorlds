using UnityEngine;
using System.Collections;

public class ToggleGroup : MonoBehaviour {

	[SerializeField] ToggleButton[] buttons;
	public string value;
	public void SetButtonActive(ToggleButton b){
		SetAllInactive ();
		this.value = b.value;
		b.SetActiveColor ();
	}
	public void SetAllInactive(){
		this.value = "";
		foreach (ToggleButton tb in buttons) {
			tb.SetInactiveColor();
		}
	}
}
