using UnityEngine;
using System.Collections;



public class CharacterManager : MonoBehaviour {

	private static CharacterManager s_Instance;



	[SerializeField] Character[] _gameCharacters;

	[SerializeField] string[] _firstName;
	[SerializeField] string[] _secondName;
	[SerializeField] Transform[] characterSpawnPlaces;

	public static Transform[] CharacterSpawnPlaces{
		get{
			return s_Instance.characterSpawnPlaces;
		}
		set{
			s_Instance.characterSpawnPlaces = value;
		}
	}
	public static Character SelectedCharacter;
	
	public static Character[] gameCharacters{
		set{s_Instance._gameCharacters = value;}
		get{ return s_Instance._gameCharacters;}
	}



	public static bool partyFull{
		get{
			int count = 0;
			foreach(Character c in gameCharacters){
				if(c.inFightingParty) count ++;
			}
			return count >=4;
		}
	}
	public static bool partyEmpty{
		get{
			int count = 0;
			foreach(Character c in gameCharacters){
				if(c.inFightingParty) count ++;
			}
			return count == 0;
		}
	}
	
	private void Awake(){
		s_Instance = this;
	}
	private void Start(){
		CreateCharacters();
		LoadCharactersFromBattle ();
	}
	private static void CreateCharacters(){
		foreach(Character c in gameCharacters){
			c.CreateCharacter();
		}
		CheckLabels (LabelManager.checkFirstWhenComeFromBattle);
		CheckLabels (LabelManager.checkSecondWhenComeFromBattle);
		CheckLabels (LabelManager.checkWhenWeaponsChange);
		CheckLabels (LabelManager.checkAfterBuildingUpdate);


	}

	public static void LoadCharactersFromBattle(){
		for (int i = 0; i<gameCharacters.Length; i++) {
			Character c = gameCharacters[i];
			c.transform.parent = s_Instance.characterSpawnPlaces[i].transform;
			c.Looks.StartAnimation(AnimationNames.kWalk);
			Vector3 lPos = c.transform.localPosition;
			lPos.z = 0;
			c.transform.localPosition = lPos;
			c.MoveCharacterToPosition(c.transform.parent.position,delegate(Character character) {
				character.Looks.StopAnimation();
			});
			c.Health += c.MaxHealth/2;
		}
	}
	
	public static string GenerateCharacterName(){
		return s_Instance._firstName[(int)Random.Range(0,s_Instance._firstName.Length)] + " " +
			   s_Instance._secondName[(int)Random.Range(0,s_Instance._secondName.Length)];
	}

	public static void MoveAllHere(Vector2 position){
		foreach (Character c in gameCharacters) {
			c.MoveCharacterToPosition(position,delegate(Character character) {});
		}
	}
	public static void MoveAllActiveHere(Vector2 position){
		foreach (Character c in gameCharacters) {
			if(c.inFightingParty)c.MoveCharacterToPosition(position,delegate(Character character) {});
		}
	}
	public static bool CheckAllActiveState(FightState state){
		foreach (Character c in gameCharacters) {
			if(c.inFightingParty && c.fightState != state){
				return false;
			}
		}
		return true;
	}

	public static void MoveAllActiveHereAndChangeState(Vector2 position,FightState state, VOID_FUNCTION callback = null){
		foreach (Character c in gameCharacters) {
			if(c.inFightingParty){

					c.MoveCharacterToPosition(position, delegate(Character character) {
				
					character.fightState = state;
					if(callback!=null){
						if(CheckAllActiveState(state)){

							callback();
						}
					}
				});
			}
		}
	}
	public static void ChangeAllStateTo(FightState state){
		foreach (Character c in gameCharacters) {
			c.fightState = state;
		}
	}
	public static void ChangeAllActiveStateTo(FightState state){
		foreach (Character c in gameCharacters) {
			if(c.inFightingParty)c.fightState = state;
		}
	}
	public static void CleanUpAfterFight(){
		CheckLabels (LabelManager.checkFirstWhenComeFromBattle);
		CheckLabels (LabelManager.checkSecondWhenComeFromBattle);
		foreach (Character c in gameCharacters) {
			Debug.Log(c.Name);
			if(c.inFightingParty){
				c.CleanUpAfterFight();
			
			}else{
				c.foughtInLastBattle = false;
			}
		}
	}

	public static void CheckLabels(LabelManager.LabelType[] labelGroup){
		foreach (Character c in gameCharacters) {
			foreach(LabelManager.LabelType lt in labelGroup){
				if(c.labels.ContainsKey(lt)){
					Label testLabel = c.labels[lt];
					if(!testLabel.isActive(c)){
						c.labels.Remove(lt);
					}
				}else if(LabelManager.GetLabel(lt).isActive(c)){
					c.labels.Add(lt,new Label(LabelManager.GetLabel(lt)));
				}
			}
		}
	}


}
