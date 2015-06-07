using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MultipleButton : MonoBehaviour {
	
	
	[SerializeField] MultipleGroup group;
	[SerializeField] Color activeColor = Color.green, inactiveColor = Color.white;
	[HideInInspector]public string value;
	public bool isSelected;
	
	private void Start(){
		this.SetInactiveColor ();
		if (string.IsNullOrEmpty (value)) {
			value = GetComponentInChildren<Text> ().text;
		}
	}
	
	public void OnClick(){
		if (isSelected) {
			isSelected = false;
			SetInactiveColor ();
		}
		else {
			if(group.GetSelectedCount() <5){
				isSelected = true;
				SetActiveColor();
			}
		}
	}
	
	public void SetActiveColor(){
		this.GetComponent<Image> ().color = activeColor;
	}
	public void SetInactiveColor(){
		this.GetComponent<Image> ().color = inactiveColor;
	}
	
	
	
}
