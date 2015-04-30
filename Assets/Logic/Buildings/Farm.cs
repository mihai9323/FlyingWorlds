using UnityEngine;
using System.Collections;

public class Farm : MonoBehaviour {
	[SerializeField] UpgradeButton upgradeButton;

	public int farmLevel = 1;
	public int upgradeCost{
		get{
			return (int)((farmLevel + 1) * 10 * Mathf.Pow(2,farmLevel+1));
		}
	}
	public int incomePerTurn{
		get{
			return (int)(upgradeCost*.8f);
		}
	}
	[HideInInspector]public void Upgrade(){
		if (GameData.Pay (upgradeCost)) {
			farmLevel++;
			upgradeButton.gameObject.SetActive(false);
		}
		ShowUpgradeButton ();
	}

	public void ShowUpgradeButton(){
		upgradeButton.SetText ("Upgrade to level "+(farmLevel+1).ToString()+" for "+upgradeCost.ToString()+" and earn "+incomePerTurn.ToString()+"per turn");
		upgradeButton.gameObject.SetActive(true);
		StopCoroutine ("HideUpgrade");
		StartCoroutine ("HideUpgrade", 3.0f);
	}
	private IEnumerator HideUpgrade(float time){
		yield return new WaitForSeconds (time);
		upgradeButton.gameObject.SetActive(false);
	}
}
