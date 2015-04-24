using UnityEngine;
using System.Collections;

public class Farm : MonoBehaviour {
	[SerializeField] GameObject upgradeButton;
	
	public void ShowUpgradeButton(){
		upgradeButton.SetActive(true);
		Invoke ("HideUpgrade",3.0f);
	}
	private void HideUpgrade(){
		upgradeButton.SetActive(false);
	}
}
