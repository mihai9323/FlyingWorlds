using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ResourceBar : MonoBehaviour {

	[SerializeField] Text T_coins;

	private static Text CoinsTextField;

	private void Start(){
		CoinsTextField = T_coins;
		SetCoinCount ();
	}
	public static void SetCoinCount(){
		CoinsTextField.text = GameData.NumberOfCoins.ToString() ;
	}

}
