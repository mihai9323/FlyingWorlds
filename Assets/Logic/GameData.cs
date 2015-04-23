using UnityEngine;
using System.Collections;

public class GameData:MonoBehaviour  {

	private static GameData s_Instance;

	public GameObject hubScene, fightScene;

	private void Start(){
		s_Instance = this;
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
		if (scene == GameScenes.Hub) {
			HubManager.interactable = true;
			s_Instance.hubScene.SetActive (true);
			s_Instance.fightScene.SetActive (false);
		} else if(scene == GameScenes.Fight) {
			s_Instance.hubScene.SetActive (false);
			s_Instance.fightScene.SetActive (true);
		}
	}

	private static int numberOfCoins = 30000;
}
