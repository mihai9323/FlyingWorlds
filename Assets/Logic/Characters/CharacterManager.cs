using UnityEngine;
using System.Collections;



public class CharacterManager : MonoBehaviour {

	private static CharacterManager s_Instance;

	public static event VOID_FUNCTION_VECTOR2 onAllMoveHere;
	public static event VOID_FUNCTION_VECTOR2 onAllActiveMoveHere;

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
		if (onAllMoveHere != null)
			onAllMoveHere (position);
	}
	public static void MoveAllActiveHere(Vector2 position){
		if (onAllActiveMoveHere != null)
			onAllActiveMoveHere (position);
	}
}
