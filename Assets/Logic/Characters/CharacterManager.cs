using UnityEngine;
using System.Collections;



public class CharacterManager : MonoBehaviour {

	private static CharacterManager s_Instance;



	[SerializeField] Character[] _gameCharacters;

	[SerializeField] string[] _firstName;
	[SerializeField] string[] _secondName;
	
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
	}
	private static void CreateCharacters(){
		foreach(Character c in gameCharacters){
			c.CreateCharacter();
		}
	}
	
	public static string GenerateCharacterName(){
		return s_Instance._firstName[(int)Random.Range(0,s_Instance._firstName.Length)] + " " +
			   s_Instance._secondName[(int)Random.Range(0,s_Instance._secondName.Length)];
	}

	public static void MoveAllHere(Vector2 position){
		foreach (Character c in gameCharacters) {
			c.MoveCharacterToPosition(position);
		}
	}
	public static void MoveAllActiveHere(Vector2 position){
		foreach (Character c in gameCharacters) {
			if(c.inFightingParty)c.MoveCharacterToPosition(position);
		}
	}
	public static bool CheckAllActiveState(Character.FightState state){
		foreach (Character c in gameCharacters) {
			if(c.inFightingParty && c.fightState != state){
				return false;
			}
		}
		return true;
	}

	public static void MoveAllActiveHereAndChangeState(Vector2 position,Character.FightState state, VOID_FUNCTION callback = null){
		foreach (Character c in gameCharacters) {
			if(c.inFightingParty){
				Debug.Log("Logged name:"+c.Name);
					c.MoveCharacterToPosition(position, delegate(Character character) {
					Debug.Log("Character reached destination callback"+character.Name);
					character.fightState = state;
					if(callback!=null){
						if(CheckAllActiveState(state)){
							Debug.Log("All characters are active checked");
							callback();
						}
					}
				});
			}
		}
	}
	public static void ChangeAllStateTo(Character.FightState state){
		foreach (Character c in gameCharacters) {
			c.fightState = state;
		}
	}
	public static void ChangeAllActiveStateTo(Character.FightState state){
		foreach (Character c in gameCharacters) {
			if(c.inFightingParty)c.fightState = state;
		}
	}


}
