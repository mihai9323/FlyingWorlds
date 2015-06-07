using UnityEngine;
using System.Collections;

public class GameData:MonoBehaviour  {

	private static GameData s_Instance;
	private static int numberOfCoins;
	public GameObject hubScene, fightScene;

	public static int Progression = 1;
	[SerializeField] string _nextBattleID;

	public static string prevBattleID;
	public static string nextBattleID{ get { return s_Instance._nextBattleID; } set { if(value != s_Instance._nextBattleID) prevBattleID = s_Instance._nextBattleID; s_Instance._nextBattleID = value; } }
	private static GameScenes _currentScene;
	public static GameScenes currentScene{
		get{
			return _currentScene;
		}
	}
	private void Awake(){
		s_Instance = this;
		numberOfCoins = 2000;
		Progression = 1;
		_currentScene = GameScenes.Hub;
	}
	private void Start(){


		prevBattleID = nextBattleID;
		_currentScene = GameScenes.None;
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
		GameScenes prevScene = _currentScene;
		_currentScene = scene;
		if (scene == GameScenes.Hub) {
			HubManager.interactable = true;
			s_Instance.hubScene.SetActive (true);
			s_Instance.fightScene.SetActive (false);
			if(prevScene == GameScenes.Fight){
				ComeBackFromBattle();
			}
			HubManager.ShowQuestInfo();
		} else if(scene == GameScenes.Fight) {
			s_Instance.hubScene.SetActive (false);
			s_Instance.fightScene.SetActive (true);
		}

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
	
		NumberOfCoins += HubManager.farm.incomePerTurn;
		CharacterManager.LoadCharactersFromBattle ();
		InventoryManager.GenerateShopItems (12, HubManager.shop.level * 5, HubManager.shop.level * 10);
	}


}
