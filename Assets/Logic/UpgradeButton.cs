using UnityEngine;
using System.Collections;


public class UpgradeButton : MonoBehaviour {

	public void UpgradeFarm(){
		this.gameObject.SetActive(false);
		Debug.Log("Farm upgraded");
	}
	public void UpgradeMine(){
		this.gameObject.SetActive(false);
		Debug.Log("Mine upgraded");
	}
	
}
