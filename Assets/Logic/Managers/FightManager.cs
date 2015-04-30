using UnityEngine;
using System.Collections;

public class FightManager : MonoBehaviour {
	private static FightManager s_Instance;
	[SerializeField] Transform minSpawn, maxSpawn;
	[SerializeField] Enemy[] enemyTypes;
	[SerializeField] Projectile _fireBall, _arrow;
	public static Transform s_transform{
		get{
			return s_Instance.transform;
		}
	}
	public static Projectile fireBall{
		get{
			return s_Instance._fireBall;
		}
	}
	public static Projectile arrow{
		get{
			return s_Instance._arrow;
		}
	}
	private void Awake(){
		s_Instance = this;
	}
	private void OnEnable(){
		Vector3 mPos = s_Instance.minSpawn.position;
		Vector3 MPos = s_Instance.maxSpawn.position;
		EnemyManager.GenerateEnemies (new float[3]{.2f,.5f,.3f},20,mPos,MPos,enemyTypes);
		LoadCharactersInScene ();
		StartCharactersAI ();
	}
	[SerializeField] Transform fleeTarget;
	public static Transform FleeTarget{
		get{
			return s_Instance.fleeTarget;
		}
	}
	[SerializeField] CharacterFightController[] fightControllers;
	[SerializeField] Transform[] characterSpawnPlaces;

	public static CharacterFightController[] FightControllers{
		get{
			return s_Instance.fightControllers;
		}
	}
	public static Transform[] CharacterSpawnPlaces{
		get{
			return s_Instance.characterSpawnPlaces;
		}
		set{
			s_Instance.characterSpawnPlaces = value;
		}
	}
	public static bool BattleLost{
		get{
			foreach(Character c in CharacterManager.gameCharacters){
				if(c.inFightingParty && !c.fled){
					return false;
				}
			}
			return true;
		}
	}

	public static void LoadCharactersInScene(){
		int c = 0;
		foreach(Character character in CharacterManager.gameCharacters){
			if(character.inFightingParty){

				FightControllers[c].LoadCharacterIn(character);
				character.transform.position = CharacterSpawnPlaces[c].transform.position;
				character.transform.parent = CharacterSpawnPlaces[c].transform;
				character.FaceDirection(1);
				character.fightState = FightState.StandGround;
				character.tag = "CHARACTER";
				c++;
			}
		}
		for(int i = c; i<4; i++){
			FightControllers[i].LoadCharacterIn(null);
		}
	}
	public static void StartCharactersAI(){
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.inFightingParty){
				c.StartAITick();
			}
		}
	}
	public static void StopCharactersAI(){
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.inFightingParty){
				c.StopAITick();
			}
		}
	}
	public static void CheckWin(){
		bool win = true;
		if(EnemyManager.LevelEnemies != null && EnemyManager.LevelEnemies.Length>0){
			foreach (Enemy e in EnemyManager.LevelEnemies) {
				if(e!=null && !e.dead){
					win = false;
					break;
				}
			}
		}
		if (win) {
			s_Instance.StartCoroutine(s_Instance.WinDelayed(2.0f));
		}
	}
	public static void CheckLost(){
		if(FightManager.BattleLost){
			Debug.Log("BattleLost!");
			CleanUpFight();
			GameData.LoadScene(GameScenes.Hub);
		}
	}
	public static void CleanUpFight(){
		EnemyManager.CleanUP();
		CharacterManager.CleanUpAfterFight();

	}
	private IEnumerator WinDelayed(float time){
		yield return new WaitForSeconds (time);
		Debug.Log("BattleWon!");
		CleanUpFight();
		GameData.LoadScene(GameScenes.Hub);
	}
}