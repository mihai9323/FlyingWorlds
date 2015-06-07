using UnityEngine;
using System.Collections;
[System.Serializable]
public class Label  {

	
	public string name;
	public LabelManager.LabelType labelType;
	public LabelManager.LabelType prerequisite;
	public int moraleChange;	
	public bool hasNotAccomplished{
		get{
			return _notAccomplished!= null && _notAccomplished.Length>0;
		}
	}
	[SerializeField] string[] _effectString;

	public string[] _notAccomplished;
	[HideInInspector]public string color;
	[HideInInspector]public string monster;
	[HideInInspector]public int receivedTurn;

	[HideInInspector]public string causeLocation;
	[HideInInspector]public string causeName;

	public string effectString(TraitManager.TraitTypes trait){
		string s = _effectString [Random.Range (0, _effectString.Length)];
		if (s.Contains ("[name]") && string.IsNullOrEmpty (causeName))
			return "";
		if (s.Contains ("[location]") && string.IsNullOrEmpty (causeLocation))
			return "";
		if (s.Contains ("[color]") && string.IsNullOrEmpty (color))
			return "";
		if (s.Contains ("[monster]") && string.IsNullOrEmpty (monster))
			return "";
		s = s.Replace ("[name]", causeName);
		s = s.Replace ("[location]", causeLocation);
		s = s.Replace ("[color]", color);
		s = s.Replace ("[monster]", monster);
		return s;
	}
	public string notAccomplishedString(string name, string location, string color,string monster){
		if (hasNotAccomplished) {
			string s = _notAccomplished [Random.Range (0, _notAccomplished.Length)];
			if (s.Contains ("[name]") && string.IsNullOrEmpty (name))
				return "";
			if (s.Contains ("[location]") && string.IsNullOrEmpty (location))
				return "";
			if (s.Contains ("[color]") && string.IsNullOrEmpty (color))
				return "";
			if (s.Contains ("[monster]") && string.IsNullOrEmpty (monster))
				return "";
			s = s.Replace ("[name]", name);
			s = s.Replace ("[location]", location);
			s = s.Replace ("[color]", color);
			s = s.Replace ("[monster]", monster);
			return s; 
		}
		return "";
	}

	public Label(Label l, string causeName = "", string causeLocation ="", string color = "",string monster=""){
		this.labelType = l.labelType;
		this.moraleChange = l.moraleChange;
		this._effectString = l._effectString;
		this.causeName = causeName;
		this.causeLocation = causeLocation;
		this.color = color;
		this.receivedTurn = GameData.Progression;
		this.monster = monster;
	}

	public bool isActive(Character character, out string name, out string location, out string color,out string monster){

		name = "";
		location = "";
		color = "";
		monster = "";
			switch(this.labelType){
			case LabelManager.LabelType.hasSword: return HasWeaponOfType(character,Item.ItemType.Melee,out color,out monster); break;
			case LabelManager.LabelType.hasBestSword: return HasBestWeaponOfType(character,Item.ItemType.Melee,out name,out color,out monster); break;
			case LabelManager.LabelType.hasBow: return HasWeaponOfType(character,Item.ItemType.Ranged,out color,out monster); break;
			case LabelManager.LabelType.hasBestBow: return HasBestWeaponOfType(character,Item.ItemType.Ranged,out name,out color,out monster); break;
			case LabelManager.LabelType.hasStaff: return HasWeaponOfType(character,Item.ItemType.Magic,out color,out monster); break;
			case LabelManager.LabelType.hasBestStaff: return HasBestWeaponOfType(character,Item.ItemType.Magic,out name,out color,out monster); break;
			case LabelManager.LabelType.hasBetterArmorThanWeapon: return HasBetterArmorThanWeapon(character,out color); break;
			case LabelManager.LabelType.hasBestArmor: return HasBestArmor(character,out name,out color); break;
			case LabelManager.LabelType.hasALotOfHealth: return character.Health> character.MaxHealth * .80f; break;
			case LabelManager.LabelType.wasNotHit: return character.Health == character.MaxHealth; break;
			case LabelManager.LabelType.isFighting: return character.foughtInLastBattle; break;
			case LabelManager.LabelType.dealtMostDamage: return HasDoneMostDamage(character,out name, out location); break;
			case LabelManager.LabelType.nobodyFled: return NobodyFled(out name,out location); break;
			case LabelManager.LabelType.bestGearValue: return HasBestGear(character,out name); break;
			case LabelManager.LabelType.fairness: return IsFair(character,out name); break;
			case LabelManager.LabelType.highTeamMorale: return HighTeamMorale(); break;
			case LabelManager.LabelType.lowTeamMorale: return LowTeamMorale(); break;
			case LabelManager.LabelType.goodFarm: return GoodFarm(); break;
			case LabelManager.LabelType.goodShop: return GoodShop(); break;
			case LabelManager.LabelType.noSword: return !HasWeaponOfType(character,Item.ItemType.Melee,out color,out monster); break;
			case LabelManager.LabelType.noBow: return !HasWeaponOfType(character,Item.ItemType.Ranged,out color,out monster); break;
			case LabelManager.LabelType.noStaff: return !HasWeaponOfType(character,Item.ItemType.Magic,out color,out monster); break;
			case LabelManager.LabelType.badArmor: return !HasBetterArmorThanWeapon(character,out color); break;
			case LabelManager.LabelType.hitInBattle: return character.Health< character.MaxHealth * .80f;; break;
			case LabelManager.LabelType.notFighting: return !character.foughtInLastBattle; break;
			case LabelManager.LabelType.partyMemberFled: return !NobodyFled(out name,out location); break;
			case LabelManager.LabelType.notBestGear: return !HasBestGear(character,out name); break;
			case LabelManager.LabelType.unfair: return !IsFair(character,out name); break;
			case LabelManager.LabelType.badFarm: return !GoodFarm(); break;
			case LabelManager.LabelType.badShop: return !GoodShop(); break;
			default: return false;
		}

	}

	public static bool HasWeaponOfType(Character character, Item.ItemType weaponType, out string color,out string monster){
		color = "";
		monster = "";
		if (character.WeaponItem != null) {
			color = character.WeaponItem.colorName;
			monster = character.WeaponItem.MostMonstersKilled;
			return character.WeaponItem.itemType == weaponType;
		}
		return false;
	}
	public static bool HasBestWeaponOfType(Character character, Item.ItemType weaponType, out string betterName,out string color,out string monster){
		betterName = "";
		color = "";
		monster = "";
		if (HasWeaponOfType (character, weaponType,out color,out monster)) {
			int bestValue = character.WeaponItem.Value;
			foreach(Character c in CharacterManager.gameCharacters){
				if(c.WeaponItem != null && c.WeaponItem.itemType == weaponType && c.WeaponItem.Value> bestValue && c!=character){
					betterName = c.Name;
					color = c.WeaponItem.colorName;
					monster = c.WeaponItem.MostMonstersKilled;
					return false;
				}
			}
			return true;
		} else return false;
	}
	public static bool HasBestArmor(Character character, out string betterName,out string color){
		betterName = "";
		color = "";
		if (HasBetterArmorThanWeapon (character,out color)) {
			int bestValue = character.ArmorItem.Value;
			foreach(Character c in CharacterManager.gameCharacters){
				if(c.ArmorItem != null && c.ArmorItem.itemType == Item.ItemType.Armor && c.ArmorItem.Value> bestValue && c!=character){
					betterName = c.Name;
					color = c.ArmorItem.colorName;
					return false;
				}
			}
			return true;
		} else return false;
	}
	public static bool HasBetterArmorThanWeapon(Character character,  out string color){
		color = "";
		if (character.ArmorItem != null && character.ArmorItem.itemType != Item.ItemType.None) {
			color = character.ArmorItem.colorName;
			if(character.WeaponItem != null && character.WeaponItem.itemType != Item.ItemType.None){
				if(character.ArmorItem.Value> character.WeaponItem.Value){
					color = character.ArmorItem.colorName;
					return true;
				}
			}else return true;
		}
		return false;
	}
	public static bool HasDoneMostDamage(Character character, out string name, out string location){
		name = "";
		if (FightManager.battles!=null && FightManager.battles.ContainsKey(GameData.prevBattleID)) {
			location = FightManager.battles [GameData.prevBattleID].location;
		} else
			location = "Last battle";

		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.damageDealtInLastBattle > character.damageDealtInLastBattle && c!= character){
				name = c.Name;
				return false;
			}
		}
		return true;
	}
	public static bool NobodyFled(out string fledName, out string location){
		if (DramaManager.previousQuest!=null) {
			location = DramaManager.previousQuest.location.locationName;
		} else
			location = "Last battle";
		fledName = "";
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.fled || c.labels.ContainsKey(LabelManager.LabelType.fled)){
				fledName = c.Name;
				return false;
			}
		}
		return true;
	}
	public static bool HasBestGear(Character character, out string hasBetterGear){
		int myValue = character.GearValue;
		hasBetterGear = "";
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c!= character){
				int cValue = c.GearValue;
				if(cValue>myValue){
					hasBetterGear = c.Name;
					return false;
				}
			}
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
	public static bool IsFair(Character character,out string notFairName){
		notFairName = "";
		foreach (Character c in CharacterManager.gameCharacters) {
			if(c.labels.ContainsKey(LabelManager.LabelType.bestGearValue) && Label.HasBestGear(c) && c.labels[LabelManager.LabelType.bestGearValue].receivedTurn +2  < GameData.Progression){
				notFairName = c.Name;
				if(c == character){
					notFairName = "me";
				}
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
