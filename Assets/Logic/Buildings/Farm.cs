using UnityEngine;
using System.Collections;

public class Farm : MonoBehaviour {
	[SerializeField] UpgradeButton upgradeButton;

	public int farmLevel = 1;

	private void Start(){
		farmLevel = PersistentData.previousFarmLevel;
	}

	public int upgradeCost{
		get{
			return (int)((farmLevel + 1) * 500);
		}
	}
	public float priceVariance{
		get{
			return CharacterManager.GetPartyBonus(new BuffsAndDebuffs.BuffType[1]{BuffsAndDebuffs.BuffType.FarmGeneratesMoreCoins},
			new BuffsAndDebuffs.BuffType[1]{BuffsAndDebuffs.BuffType.FarmGenerateLessCoins},1,false);
		}
	}
	public int incomePerTurn{
		get{
			return (int)(farmLevel * (400+20 * DramaPack.DramaManager.progression) * priceVariance);
		}
	}
	public int incomePerTurnNextLevel{
		get{
			return (int)((farmLevel+1) * (400+20 * DramaPack.DramaManager.progression) * priceVariance);
		}
	}
	public void Upgrade(){
		if (GameData.Pay (upgradeCost)) {
			farmLevel++;
			ResourceBar.SetCoinCount();
			CharacterManager.CheckLabels(LabelManager.checkAfterBuildingUpdate);
		} else {
			HubManager.notification.ShowNotification("Your treasury doesn't support this investment!","Oh!");
		}

	}

	public void ShowUpgradeButton(){
		if (HubManager.interactable) {
			HubManager.notification.ShowConfirm (
			"Upgrade to level " + (farmLevel + 1).ToString () + " for " + upgradeCost.ToString () + " and earn " + incomePerTurnNextLevel.ToString () + "per turn",
			"YES",
			"NO",
			delegate() {
				Upgrade ();
			}
			);
		}
	}

	
}
