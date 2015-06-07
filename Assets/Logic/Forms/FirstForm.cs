using UnityEngine;
using System.Collections;

using System;

public class FirstForm : MonoBehaviour {
	[SerializeField] string url;
	[SerializeField] string gameVersion;
	public static string seassionID;
	public static DateTime startSeassion;
	[SerializeField] UnityEngine.UI.InputField age , playTime;
	[SerializeField] ToggleGroup sex, wantToPlay,games;
	[SerializeField] GameObject notValidError, networkError;

	void Start(){
		notValidError.SetActive (false);
		networkError.SetActive (false);
		startSeassion = DateTime.UtcNow;

		seassionID = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

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
		if (string.IsNullOrEmpty (age.text)) return false;
		if (string.IsNullOrEmpty (games.value)) return false;
		if (string.IsNullOrEmpty (playTime.text)) return false;

		if (string.IsNullOrEmpty (sex.value)) return false;
		if (string.IsNullOrEmpty (wantToPlay.value)) return false;
		return true;
	}

	private IEnumerator sendData(){
		WWWForm form = new WWWForm ();

		form.AddField ("seassion_id", seassionID);
		form.AddField ("age", age.text);
		form.AddField ("favourite_games", games.value);
		form.AddField ("play_time_per_week", playTime.text);
		form.AddField ("sex", sex.value);
		form.AddField ("initial_motivation", wantToPlay.value);
		form.AddField ("game_version", gameVersion);

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
