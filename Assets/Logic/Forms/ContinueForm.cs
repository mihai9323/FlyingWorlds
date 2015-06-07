using UnityEngine;
using System.Collections;

using System;

public class ContinueForm : MonoBehaviour {
	[SerializeField] string url;


	[SerializeField] ToggleGroup wantToPlay;
	[SerializeField] MultipleGroup selectedCards;
	[SerializeField] GameObject notValidError, networkError;
	
	void Start(){
		notValidError.SetActive (false);
		networkError.SetActive (false);

		
	}
	public void SendForm(){
		if (ValidData ()) {
			notValidError.SetActive(false);
			StartCoroutine(sendData());
		} else {
			notValidError.SetActive(true);
		}
	}
	private bool ValidData(){
	
		
		if (selectedCards.getValues() == null || selectedCards.getValues().Length != 5)
			return false; 
		if (string.IsNullOrEmpty (wantToPlay.value)) return false;
		return true;
	}
	
	private IEnumerator sendData(){
		WWWForm form = new WWWForm ();
		
		form.AddField ("seassion_id", FirstForm.seassionID);
		string[] values = selectedCards.getValues ();
		for (int i = 0; i<values.Length; i++) {
			form.AddField ("card"+i, values[i]);
		}
	
		form.AddField ("motivation", wantToPlay.value);
		
		WWW www = new WWW (url, form);
		this.transform.GetChild (0).gameObject.SetActive (false);
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) {
			Debug.Log(www.error);
			networkError.SetActive(true);
			this.transform.GetChild (0).gameObject.SetActive (true);
		}
		else {
			Debug.Log("Finished Uploading Data");
			Application.LoadLevel(Application.loadedLevel+1);
		}
	}
}
