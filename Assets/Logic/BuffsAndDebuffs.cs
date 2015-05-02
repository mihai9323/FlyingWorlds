using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BuffsAndDebuffs : MonoBehaviour {

	public enum BuffType{
		MoreMeleeDamageSelf,
		MoreBowDamageSelf,
		MoreMagicDamageSelf,
		MoreDefenseSelf,
		TakesLessDamageParty,
		GivesMoreDamageParty,
		LessFleeParty,
		PrimeTargetSelf,
		MoreMoraleWorstGear,
		MoreMoraleParty,
		MoreDamgeSelf,
		FarmGeneratesMoreCoins,
		ShopDiscount,

		LessMeleeDamageSelf,
		LessBowDamageSelf,
		LessMagicDamageSelf,
		LessDefenseSelf,
		MoreDamageTakenParty,
		LessDamageGivenParty,
		MoreFleeParty,
		LessMoraleForBestGear,
		StandGround,
		LessMoraleParty,
		LessDamageSelf,
		FarmGenerateLessCoins,
		ShopMoreExpensive
	}

	public Buff[] _buffDefinitions;
	public static Dictionary<BuffsAndDebuffs.BuffType,Buff> buffDefinitions;

	public void Awake(){

		buffDefinitions = new Dictionary<BuffsAndDebuffs.BuffType, Buff> (_buffDefinitions.Length);
		foreach(Buff b in _buffDefinitions){
			buffDefinitions.Add(b.buffType,b);
		}
	}
	

}
[System.Serializable]
public class Buff{
	public string name;
	public BuffsAndDebuffs.BuffType buffType;
	public float maxEffectPercentValue;
	public float maxAdditiveValue;
	public bool doesntFight;
}
