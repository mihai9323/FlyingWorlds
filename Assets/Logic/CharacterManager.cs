using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	private static CharacterManager s_Instance;
	
	[SerializeField] Character[] _gameCharacters;
	[SerializeField] string[] _firstName;
	[SerializeField] string[] _secondName;
	
	public static Character[] gameCharacters{
		set{s_Instance._gameCharacters = value;}
		get{ return s_Instance._gameCharacters;}
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
}
