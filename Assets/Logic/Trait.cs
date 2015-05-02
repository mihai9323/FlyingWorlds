using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Trait {

	public string name;
	public TraitManager.TraitTypes traitType;

	public List<BuffsAndDebuffs.BuffType> buffs;
	public List<BuffsAndDebuffs.BuffType> debuffs;

	public List<LabelManager.LabelType> influencedBy;


}
