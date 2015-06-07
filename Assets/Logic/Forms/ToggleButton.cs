using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ToggleButton : MonoBehaviour {


	[SerializeField] ToggleGroup toggleGroup;
	[SerializeField] Color activeColor = Color.green, inactiveColor = Color.white;
	public string value;

	private void Start(){
		this.SetInactiveColor ();
		if (string.IsNullOrEmpty (this.transform.GetComponentInChildren<Text> ().text)) {
			this.transform.GetComponentInChildren<Text>().text = value;
		}
	}

	public void OnClick(){
		toggleGroup.SetButtonActive (this);
	}

	public void SetActiveColor(){
		this.GetComponent<Image> ().color = activeColor;
	}
	public void SetInactiveColor(){
		this.GetComponent<Image> ().color = inactiveColor;
	}



}
