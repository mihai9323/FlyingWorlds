using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ResourceBar : MonoBehaviour {

	[SerializeField] Text T_coins;
	[SerializeField] Text T_income;
	private static Text CoinsTextField;
	private static Text IncomeTextField;
	private IEnumerator Start(){
		yield return null;
		CoinsTextField = T_coins;
		IncomeTextField = T_income;
		SetCoinCount ();
	}
	public static void SetCoinCount(){
		CoinsTextField.text = GameData.NumberOfCoins.ToString() ;
		IncomeTextField.text = "(+"+HubManager.farm.incomePerTurn+")";

	}

}
