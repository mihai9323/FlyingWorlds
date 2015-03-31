using UnityEngine;
using System.Collections;
[System.Serializable]
public class Trait {

	public string name;
	public TraitManager.TraitTypes traitType;
	public float healthBonus;
	public float armorBonus;
	public float meleeBonus;
	public float rangedBonus;
	public float magicBonus;
	
	public float moraleBonus;
	
	public bool immuneToMagic;
	
	public float orderResponse;
	public LabelManager.LabelType lifeGoal;
	public LabelManager.LabelType[] hateOtherLabel;
	public LabelManager.LabelType[] hateSelfLabel;
}
