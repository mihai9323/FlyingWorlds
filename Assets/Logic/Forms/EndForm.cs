using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndForm : MonoBehaviour {
	[SerializeField] string url;
	[SerializeField] UnityEngine.UI.InputField feedback;
	[SerializeField] ToggleGroup q1,q2;
	[SerializeField] GameObject credits;
	[SerializeField] GameObject childContainer;
	public void OnClick(){
		if (Valid()) {
			StartCoroutine(sendFeedback());
		}
	}

	private IEnumerator sendFeedback(){
		WWWForm form = new WWWForm ();
		form.AddField ("seassion_id", FirstForm.seassionID);
		form.AddField ("story1", q1.value);
		form.AddField ("story2", q2.value);
		WWW www = new WWW (url, form);

		this.transform.GetChild (0).gameObject.SetActive (false);
		yield return www;

		form = new WWWForm ();
		form.AddField ("seassion_id", FirstForm.seassionID);
		form.AddField ("feedback", feedback.text);
		form.AddField ("story1", q1.value);
		form.AddField ("story2", q2.value);
		www = new WWW (url, form);
		yield return www;
		childContainer.SetActive (false);
		credits.SetActive (true);
		yield return new WaitForSeconds (5.0f);
		Application.Quit ();
	}
	private bool Valid(){
		if (string.IsNullOrEmpty(q1.value) || string.IsNullOrEmpty(q2.value)) {
			return false;
		}
		var sendMe = feedback.text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">","&gt;").Replace("\"", "&quot;").Replace("#","hashTag ").Replace("\'","&quot;");
		feedback.text = sendMe;
		return true;
	}
}
