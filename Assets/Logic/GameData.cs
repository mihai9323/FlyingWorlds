using UnityEngine;
using System.Collections;

public class GameData:MonoBehaviour  {

	private static GameData s_Instance;

	public GameObject hubScene, fightScene;

	public static int TurnNumber = 1;
	private static GameScenes currentScene;
	private void Start(){
		s_Instance = this;
		currentScene = GameScenes.None;
		LoadScene (GameScenes.Hub);

	}
	public static int NumberOfCoins{
		set{
			numberOfCoins = value;
			ResourceBar.SetCoinCount();
		}
		get{
			return numberOfCoins;
		}
	}
	public static void LoadScene(GameScenes scene){
		GameScenes prevScene = currentScene;
		if (scene == GameScenes.Hub) {
			HubManager.interactable = true;
			s_Instance.hubScene.SetActive (true);
			s_Instance.fightScene.SetActive (false);
			if(prevScene == GameScenes.Fight){
				ComeBackFromBattle();
			}
		} else if(scene == GameScenes.Fight) {
			s_Instance.hubScene.SetActive (false);
			s_Instance.fightScene.SetActive (true);
		}
		currentScene = scene;
	}
	public static bool Pay(int coins){
		if(HasCoins(coins)){
			NumberOfCoins -= coins;
			return true;
		}
		return false;
	}
	public static bool HasCoins(int coins){
		if(NumberOfCoins>=coins){
			return true;
		}
		return false;
	}

	public static void ComeBackFromBattle(){
		TurnNumber ++;
		NumberOfCoins += HubManager.farm.incomePerTurn;
		CharacterManager.LoadCharactersFromBattle ();
		InventoryManager.GenerateShopItems (12, HubManager.shop.level * 5, HubManager.shop.level * 10);
	}

	private static int numberOfCoins = 1000;
}
