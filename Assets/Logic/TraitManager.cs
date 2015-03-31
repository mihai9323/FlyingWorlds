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
		Fatty,
		Jolly,
		Persuasive
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
	public static TraitTypes[] GetRandomTraitsTypes(int number){
		TraitTypes[] traits = new TraitTypes[number];
		for(int c =0 ; c<number;c++){
			bool ok = true;
			TraitTypes rT = (TraitTypes)((int)Random.Range(0,9));
			foreach(TraitTypes t in traits){
				if(t == rT){
					ok = false;
				}
			}
			while(!ok){
				ok = true;
				rT = (TraitTypes)((int)Random.Range(0,9));
				foreach(TraitTypes t in traits){
					if(t == rT){
						ok = false;
					}
				}
			}
			traits[c] = rT;
		}
		return traits;
	}
}
