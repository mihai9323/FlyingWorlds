using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NotificationBox : MonoBehaviour {
	[SerializeField] Text T_OkBtn;
	[SerializeField] Text T_NoBtn;
	[SerializeField] Text T_Notification;
	[SerializeField] GameObject yesButton;
	[SerializeField] GameObject noButton;

	public event VOID_FUNCTION OnYesClicked,OnNoClicked;

	public void ShowNotification(string text, string cancelButton="cancel", VOID_FUNCTION cancelCallback=null){
		T_Notification.text = text;
		T_NoBtn.text = cancelButton;
		yesButton.SetActive (false);
		noButton.SetActive (true);
		OnNoClicked = null;
		if (cancelCallback != null) {
			OnNoClicked += cancelCallback;
		}
		this.gameObject.SetActive (true);
	}
	public void ShowConfirm(string text, string confirmButton="yes", string cancelButton="no",VOID_FUNCTION confirmCallback=null, VOID_FUNCTION cancelCallback=null){
		ShowNotification (text, cancelButton, cancelCallback);
		T_OkBtn.text = confirmButton;
		yesButton.SetActive (true);
		OnYesClicked = null;
		if (confirmCallback != null) {
			OnYesClicked += confirmCallback;
		}
	}
	public void Hide(){

		this.gameObject.SetActive (false);
	}
	public void OkClicked(){
		Hide ();
		if (OnYesClicked != null) {
			OnYesClicked();
		}


	}
	public void CancelClicked(){
		Hide ();
		if (OnNoClicked != null) {
			OnNoClicked();
		}

	}

}
