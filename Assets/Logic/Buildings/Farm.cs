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
	public void Upgrade(){
		if (GameData.Pay (upgradeCost)) {
			farmLevel++;
		} else {
			HubManager.notification.ShowNotification("Your treasury doesn't support this investment!","Oh!");
		}

	}

	public void ShowUpgradeButton(){
		HubManager.notification.ShowConfirm(
			"Upgrade to level "+(farmLevel+1).ToString()+" for "+upgradeCost.ToString()+" and earn "+incomePerTurn.ToString()+"per turn",
			"YES",
			"NO",
			delegate() {
				Upgrade();
			}
		);
	}

	
}
