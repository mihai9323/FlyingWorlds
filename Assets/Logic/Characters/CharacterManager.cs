using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CharacterManager : MonoBehaviour {

	private static CharacterManager s_Instance;

	[SerializeField] bool _generateCharacters;
	public static bool generateCharacters{
		get{
			return s_Instance._generateCharacters;
		}
	}

	[SerializeField] Character[] _gameCharacters;

	[SerializeField] string[] _firstName;

	[SerializeField] Transform[] characterSpawnPlaces;
	public static Dictionary<TraitManager.TraitTypes,List<Character>> charactersByTrait {
		get{ if(s_Instance._charactersByTrait == null){
				GetCharactersByTraits();
			} 
			return s_Instance._charactersByTrait;
		}
	}
	private Dictionary<TraitManager.TraitTypes,List<Character>> _charactersByTrait;
	public static int partyMoral{
		get{
			return Mathf.RoundToInt(
				GetPartyBonus(new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.MoreMoraleParty,BuffsAndDebuffs.BuffType.LessMoraleParty},
							  new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.MoreMoraleParty,BuffsAndDebuffs.BuffType.LessMoraleParty},0,true)
			);
		}
	}
	public static float partyDamageBonus{
		get{
			return GetPartyBonus(new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.GivesMoreDamageParty,BuffsAndDebuffs.BuffType.LessDamageGivenParty},
								 new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.GivesMoreDamageParty,BuffsAndDebuffs.BuffType.LessDamageGivenParty},1,false);
		}
	}
	public static float partyFlee{
		get{
			return GetPartyBonus(new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.LessFleeParty,BuffsAndDebuffs.BuffType.MoreFleeParty},
				 				 new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.LessFleeParty,BuffsAndDebuffs.BuffType.MoreFleeParty},1,false);
		}
	}
	public static float partyDefence{
		get{
			return GetPartyBonus(new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.TakesLessDamageParty, BuffsAndDebuffs.BuffType.MoreDamageTakenParty},
								 new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.TakesLessDamageParty,BuffsAndDebuffs.BuffType.MoreDamageTakenParty},1,false);
		}
	}
	public static int worstGearMoraleBonus{
		get{
			return Mathf.RoundToInt(
				GetPartyBonus(new BuffsAndDebuffs.BuffType[1]{BuffsAndDebuffs.BuffType.MoreMoraleWorstGear},
							  new BuffsAndDebuffs.BuffType[1]{BuffsAndDebuffs.BuffType.MoreMoraleWorstGear}, 0,true)
				);
		}
	}
	public static int bestGearMoraleBonus{
		get{
			return Mathf.RoundToInt(
				GetPartyBonus(new BuffsAndDebuffs.BuffType[1]{BuffsAndDebuffs.BuffType.LessMoraleForBestGear},
							  new BuffsAndDebuffs.BuffType[1]{BuffsAndDebuffs.BuffType.LessMoraleForBestGear}, 0,true)
				);
		}
	}
	public static void SetAllCharactersStaticProperties(){
		foreach (Character character in gameCharacters) {
			character.SetStaticStats();
		}
	}
	/// <summary>
	/// Gets the party bonus.
	/// </summary>
	/// <returns>The party bonus.</returns>
	/// <param name="influenceBuffs">Influence buffs.</param>
	/// <param name="mode">Mode 0 = additive, 1 = multiply</param>
	public static float GetPartyBonus(BuffsAndDebuffs.BuffType[] buffs, BuffsAndDebuffs.BuffType[] debuffs, byte mode, bool plainMorale){
		float v = mode;
		foreach(Character c in gameCharacters){
			float morale = 0;
			if(plainMorale) morale = c.CalculatePlainMoral();
			else morale = c.CalculateMoral();
			v+= c.BuffInfluence(buffs,debuffs,mode,morale)-mode;
		}
		return v;
	}

	public static Character hasBestGear, hasWorstGear;

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

	public static void CalculateBestAndWorstGear(){
		int bestValue = 0;
		int worstValue = int.MaxValue;
		Character bG = null, wG=null;
		foreach (Character c in gameCharacters) {
			if(c.GearValue>bestValue){
				bestValue  = c.GearValue;
				bG = c;
			}
			if(c.GearValue<worstValue){
				worstValue = c.GearValue;
				wG = c;
			}
		}
		hasBestGear = bG;
		hasWorstGear = wG;
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
		CalculateBestAndWorstGear ();
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
	
	public static string GenerateCharacterName(Trait trait0, Trait trait1){
		return trait1.name+" "+s_Instance._firstName [(int)Random.Range (0, s_Instance._firstName.Length)]+" the "+trait0.name;
			 
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
				string name="",location="",color="",monster="";
				if(c.labels.ContainsKey(lt)){
					Label testLabel = c.labels[lt];
					if(!testLabel.isActive(c,out name,out location,out color,out monster)){
						c.labels.Remove(lt);
					}else{
						if(name != "" && testLabel.causeName != name){
							testLabel.causeName = name;
						}
						if(location != "" && testLabel.causeLocation != location){
							testLabel.causeLocation = location;
						}
						if(color != "" && testLabel.color != color){
							testLabel.color = color;
						}
						if(monster != "" && testLabel.monster != monster){
							testLabel.monster = monster;
						}
					}
				}else if(LabelManager.GetLabel(lt).isActive(c,out name,out location,out color,out monster)){
					if(c.Name == name) name = "me";
					c.labels.Add(lt,new Label(LabelManager.GetLabel(lt),name,location,color,monster));

				}
			}
		}
	}
	private static void GetCharactersByTraits(){
		s_Instance._charactersByTrait = new Dictionary<TraitManager.TraitTypes, List<Character>> ();
		foreach (Character c in gameCharacters) {
			if(s_Instance._charactersByTrait.ContainsKey(c.Traits[0])) s_Instance._charactersByTrait[c.Traits[0]].Add(c);
			else{
				s_Instance._charactersByTrait.Add(c.Traits[0],new List<Character>());
				s_Instance._charactersByTrait[c.Traits[0]].Add(c);
			}
			if(s_Instance._charactersByTrait.ContainsKey(c.Traits[1])) s_Instance._charactersByTrait[c.Traits[1]].Add(c);
			else{
				s_Instance._charactersByTrait.Add(c.Traits[1],new List<Character>());
				s_Instance._charactersByTrait[c.Traits[1]].Add(c);
			}
		}
	}

}
