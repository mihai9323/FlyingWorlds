using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TraitManager : MonoBehaviour {

	private static TraitManager s_Instance;
	
	public enum TraitTypes{
		DoesntBelieveInMagic,
		Gypsy,
		Fabulous,
		ByTheBooks,
		Scared,
		Drunk,
		Fatty
	}
	
	[SerializeField] Trait[] traits;
	
	private static Dictionary<TraitTypes,Trait> traitDictionary;
	
	private void Start(){
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
}
