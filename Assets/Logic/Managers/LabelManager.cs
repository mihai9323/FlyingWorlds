using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LabelManager : MonoBehaviour {

	private static LabelManager s_Instance;
	
	public enum LabelType{
		hasSword,
		hasBestSword,
		hasBow,
		hasBestBow,
		hasStaff,
		hasBestStaff,
		hasBetterArmorThanWeapon,
		hasBestArmor,
		hasALotOfHealth,
		wasNotHit,
		isFighting,
		dealtMostDamage,
		nobodyFled,
		bestGearValue,
		fairness,
		highTeamMorale,
		lowTeamMorale,
		goodFarm,
		goodShop,
		noSword,
		noBow,
		noStaff,
		badArmor,
		hitInBattle,
		notFighting,
		partyMemberFled,
		notBestGear,
		unfair,
		badFarm,
		badShop,
		fled,
		none

		
	}

	[SerializeField] LabelType[] _checkWhenWeaponsChange;
	[SerializeField] LabelType[] _checkFirstWhenComeFromBattle;
	[SerializeField] LabelType[] _checkSecondWhenComeFromBattle;
	[SerializeField] LabelType[] _checkAfterBuildingUpdate;

	[SerializeField] Label[] labels;
	
	private static Dictionary<LabelType,Label> labelDictionary;

	public static LabelType[] checkWhenWeaponsChange;
	public static LabelType[] checkFirstWhenComeFromBattle;
	public static LabelType[] checkSecondWhenComeFromBattle;
	public static LabelType[] checkAfterBuildingUpdate;
	
	private void Awake(){
		labelDictionary = new Dictionary<LabelType, Label>();
		foreach(Label l in labels){
			labelDictionary.Add(l.labelType,l);
		}
		checkWhenWeaponsChange = _checkWhenWeaponsChange;
		checkAfterBuildingUpdate = _checkAfterBuildingUpdate;
		checkFirstWhenComeFromBattle = _checkFirstWhenComeFromBattle;
		checkSecondWhenComeFromBattle = _checkSecondWhenComeFromBattle;
	}
	
	public static Label GetLabel(LabelType label){
		if(labelDictionary.ContainsKey(label)){
			return labelDictionary[label];
		}else return null;
	}

}
