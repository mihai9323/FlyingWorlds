using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TraitManager : MonoBehaviour {

	private static TraitManager s_Instance;
	
	public enum TraitTypes{
		Melee,
		Archery,
		Magic,
		Armory,
		Scared,
		ArmyMan,
		Fabulous,
		Brave,
		Clown,
		Pessimist,
		Farmer,
		ShopClerk,
		Fair

	}
	
	[SerializeField] Trait[] traits;
	
	private static Dictionary<TraitTypes,Trait> traitDictionary;
	private void Awake(){
		s_Instance = this;
		traitUse = new int[13];
	
		traitDictionary = new Dictionary<TraitTypes, Trait>();
		foreach(Trait t in traits){
			traitDictionary.Add(t.traitType,t);
		}
	}
	
	public static Trait GetTrait(TraitTypes trait){
		if(traitDictionary.ContainsKey(trait)){
			return traitDictionary[trait];
		}else return null;
	}
	public static TraitTypes[] GetRandomTraitsTypes(){
		TraitTypes[] traits = new TraitTypes[2];
		traits[0] = getRandomTraitType(5,13);
		traits[1] = getRandomTraitType(0,4);

		return traits;
	}
	private static TraitTypes getRandomTraitType(int fromTrait, int toTrait){
		List<int> availableTraits = new List<int>();
		int minValue = 8;
		for(int i = fromTrait; i<toTrait; i++){
			if(traitUse[i]<minValue){
				minValue = traitUse[i];
			}
		}
		for(int i = fromTrait; i<toTrait; i++){
			if(traitUse[i] == minValue){
				minValue = traitUse[i];
				availableTraits.Add(i);
			}
		}
		int rV = (int)Random.Range(0,availableTraits.Count);
		traitUse [availableTraits [rV]]++;
		return s_Instance.traits[availableTraits[rV]].traitType;
		
	}
	private static int[] traitUse;
}
