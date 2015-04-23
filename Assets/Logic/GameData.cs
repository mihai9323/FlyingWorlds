using UnityEngine;
using System.Collections;

public class GameData  {



	public static int NumberOfCoins{
		set{
			numberOfCoins = value;
			ResourceBar.SetCoinCount();
		}
		get{
			return numberOfCoins;
		}
	}
	private static int numberOfCoins = 30000;
}
