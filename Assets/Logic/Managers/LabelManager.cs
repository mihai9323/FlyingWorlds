using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LabelManager : MonoBehaviour {

	private static LabelManager s_Instance;
	
	public enum LabelType{
		FledFromBattle,
		GotNewGear,
		HasBestMeleeWeapon,
		HasBestRangedWeapon,
		HasBestMagicWeapon,
		HasBestArmor,
		HasMostExpensiveGear,
		JockedAbout,
		MoonshineQuestCompleted,
		WeaponLost,
		MagicParty,
		
	}
	
	[SerializeField] Label[] labels;
	
	private static Dictionary<LabelType,Label> labelDictionary;
	
	private void Start(){
		labelDictionary = new Dictionary<LabelType, Label>();
		foreach(Label l in labels){
			labelDictionary.Add(l.labelType,l);
		}
	}
	
	public static Label GetLabel(LabelType label){
		if(labelDictionary.ContainsKey(label)){
			return labelDictionary[label];
		}else return null;
	}
}
