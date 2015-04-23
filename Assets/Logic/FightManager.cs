using UnityEngine;
using System.Collections;

public class FightManager : MonoBehaviour {
	private static FightManager s_Instance;

	private void Awake(){
		s_Instance = this;
	}
	private void OnEnable(){
		LoadCharactersInScene ();
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

	public static void LoadCharactersInScene(){
		int c = 0;
		foreach(Character character in CharacterManager.gameCharacters){
			if(character.inFightingParty){
				Debug.Log("load: "+character.Name);
				FightControllers[c].LoadCharacterIn(character);
				character.transform.position = CharacterSpawnPlaces[c].transform.position;
				character.transform.parent = CharacterSpawnPlaces[c].transform;
				character.FaceDirection(1);
				c++;
			}
		}
		for(int i = c; i<4; i++){
			FightControllers[i].LoadCharacterIn(null);
		}
	}
}