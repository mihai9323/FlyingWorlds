using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {
	[SerializeField] Text upgrade_text;

	public void UpgradeFarm(){
		HubManager.farm.Upgrade ();

	}
	public void UpgradeMine(){
		this.gameObject.SetActive(false);
		Debug.Log("Mine upgraded");
	}
	public void SetText(string text){
		upgrade_text.text = text;
	}
	
}
