using UnityEngine;
using System.Collections;
[System.Serializable]
public class Trait {

	public string name;
	public TraitManager.TraitTypes traitType;

	public BuffsAndDebuffs.BuffType[] buffs;
	public BuffsAndDebuffs.BuffType[] debuffs;

	public LabelManager.LabelType[] likesLabel;
	public LabelManager.LabelType[] doesntLikeLable;
}
