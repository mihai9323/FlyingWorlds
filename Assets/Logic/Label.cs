using UnityEngine;
using System.Collections;
[System.Serializable]
public class Label  {

	
	public string name;
	public LabelManager.LabelType labelType;

	public int moraleChange;	

	[SerializeField] string _effectString;


	[HideInInspector]public int receivedTurn;


	public string effectString(string name){
		return _effectString.Replace ("[name]", name);
	}

	public Label(Label l, string causeName = ""){
		this.labelType = l.labelType;
		this.moraleChange = l.moraleChange;
		this._effectString = l._effectString;


		this.receivedTurn = GameData.TurnNumber;
	}

	public bool isActive(Character character){

			

			switch(this.labelType){
			case LabelManager.LabelType.hasSword: return HasWeaponOfType(character,Item.ItemType.Melee); break;
			case LabelManager.LabelType.hasBestSword: return HasBestWeaponOfType(character,Item.ItemType.Melee); break;
			case LabelManager.LabelType.hasBow: return HasWeaponOfType(character,Item.ItemType.Ranged); break;
			case LabelManager.LabelType.hasBestBow: return HasBestWeaponOfType(character,Item.ItemType.Ranged); break;
			case LabelManager.LabelType.hasStaff: return HasWeaponOfType(character,Item.ItemType.Magic); break;
			case LabelManager.LabelType.hasBestStaff: return HasBestWeaponOfType(character,Item.ItemType.Magic); break;
			case LabelManager.LabelType.hasBetterArmorThanWeapon: return HasBetterArmorThanWeapon(character); break;
			case LabelManager.LabelType.hasBestArmor: return HasBestArmor(character); break;
			case LabelManager.LabelType.hasALotOfHealth: return character.Health> character.MaxHealth * .80f; break;
			case LabelManager.LabelType.wasNotHit: return character.Health == character.MaxHealth; break;
			case LabelManager.LabelType.isFighting: return character.foughtInLastBattle; break;
			case LabelManager.LabelType.dealtMostDamage: return HasDoneMostDamage(character); break;
			case LabelManager.LabelType.nobodyFled: return NobodyFled(); break;
			case LabelManager.LabelType.bestGearValue: return HasBestGear(character); break;
			case LabelManager.LabelType.fairness: return IsFair(); break;
			case LabelManager.LabelType.highTeamMorale: return HighTeamMorale(); break;
			case LabelManager.LabelType.lowTeamMorale: return LowTeamMorale(); break;
			case LabelManager.LabelType.goodFarm: return GoodFarm(); break;
			case LabelManager.LabelType.goodShop: return GoodShop(); break;
			case LabelManager.LabelType.noSword: return !HasWeaponOfType(character,Item.ItemType.Melee); break;
			case LabelManager.LabelType.noBow: return !HasWeaponOfType(character,Item.ItemType.Ranged); break;
			case LabelManager.LabelType.noStaff: return !HasWeaponOfType(character,Item.ItemType.Magic); break;
			case LabelManager.LabelType.badArmor: return !HasBetterArmorThanWeapon(character); break;
			case LabelManager.LabelType.hitInBattle: return character.Health< character.MaxHealth * .80f;; break;
			case LabelManager.LabelType.notFighting: return !character.foughtInLastBattle; break;
			case LabelManager.LabelType.partyMemberFled: return !NobodyFled(); break;
			case LabelManager.LabelType.notBestGear: return !HasBestGear(character); break;
			case LabelManager.LabelType.unfair: return !IsFair(); break;
			case LabelManager.LabelType.badFarm: return !GoodFarm(); break;
			case LabelManager.LabelType.badShop: return !GoodShop(); break;
			default: return false;
		}

	}

	public static bool HasWeaponOfType(Character character, Item.ItemType weaponType){
		if (character.WeaponItem != null) {
			return character.WeaponItem.itemType == weaponType;
		}
		return false;
	}
	public static bool HasBestWeaponOfType(Character character, Item.ItemType weaponType){
		if (HasWeaponOfType (character, weaponType)) {
			int bestValue = character.WeaponItem.Value;
			foreach(Character c in CharacterManager.gameCharacters){
				if(c.WeaponItem != null && c.WeaponItem.itemType == weaponType && c.WeaponItem.Value> bestValue && c!=character){
					return false;
				}
			}
			return true;
		} else return false;
	}
	public static bool HasBestArmor(Character character){
		if (HasBetterArmorThanWeapon (character)) {
			int bestValue = character.ArmorItem.Value;
			foreach(Character c in CharacterManager.gameCharacters){
				if(c.ArmorItem != null && c.ArmorItem.itemType == Item.ItemType.Armor && c.ArmorItem.Value> bestValue && c!=character){
					return false;
				}
			}
			return true;
		} else return false;
	}
	public static bool HasBetterArmorThanWeapon(Character character){
		if (character.ArmorItem != null && character.ArmorItem.itemType != Item.ItemType.None) {
			if(character.WeaponItem != null && character.WeaponItem.itemType != Item.ItemType.None){
				if(character.ArmorItem.Value> character.WeaponItem.Value){
					return true;
				}
			}else return true;
		}
		return false;
	}
	public static bool HasDoneMostDamage(Character character){
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.damageDealtInLastBattle > character.damageDealtInLastBattle && c!= character){
				return false;
			}
		}
		return true;
	}
	public static bool NobodyFled(){
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.fled || c.labels.ContainsKey(LabelManager.LabelType.fled)) return false;
		}
		return true;
	}
	public static bool HasBestGear(Character character){
		int myValue = character.GearValue;

		foreach (Character c in CharacterManager.gameCharacters) {
			if(c!= character){
				int cValue = c.GearValue;
				if(cValue>myValue){
					return false;
				}
			}
		}
		return true;
	}
	public static bool IsFair(){
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.labels.ContainsKey(LabelManager.LabelType.bestGearValue) && Label.HasBestGear(c) && c.labels[LabelManager.LabelType.bestGearValue].receivedTurn +2  > GameData.TurnNumber){
				return false;
			}
		}
		return true;
	}
	public static bool HighTeamMorale(){
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.Moral < 6){
				return false;
			}
		}
		return true;
	}
	public static bool LowTeamMorale(){
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.Moral > 4){
				return false;
			}
		}
		return true;
	}
	public static bool GoodFarm(){
		return HubManager.farm.farmLevel > 1 && HubManager.farm.farmLevel > HubManager.shop.level;
	}
	public static bool GoodShop(){
		return HubManager.shop.level > 1 && HubManager.shop.level > HubManager.farm.farmLevel;
	}
}
