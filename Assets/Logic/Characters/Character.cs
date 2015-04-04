using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public string Name;
	public TraitManager.TraitTypes[] Traits;
	public Skillset Skills;
	
	public Item WeaponItem{
		get{
			if(weaponItem == null) return new Item();
			return weaponItem;
		}
		set{
			weaponItem = value;
		}
	}
	
	public Item ArmorItem{
		get{
			if(armorItem == null) return new Item();
			return armorItem;
		}
		set{
			armorItem = value;
		}
	}
	
	[SerializeField] Item weaponItem, armorItem;
	
	public int Level;
	public LabelManager.LabelType[] Labels;
	
	
	public string Thinks{
		get{
			return iThink();
		}	
	}
	
	public int MaxHealth{
		get{
			
			return (int)(Level * (TraitManager.GetTrait(Traits[0]).healthBonus + TraitManager.GetTrait(Traits[1]).healthBonus + 1));
		}
	}
	public int Armor {
		get{
			return (int)(ArmorItem.Defence * (TraitManager.GetTrait(Traits[0]).armorBonus + TraitManager.GetTrait(Traits[1]).armorBonus + 1));
		}
	}
	public int Damage{
		get{
			if(WeaponItem.itemType == Item.ItemType.Magic){
				return (int)(((Level + Skills.magic) * (TraitManager.GetTrait(Traits[0]).magicBonus + TraitManager.GetTrait(Traits[1]).magicBonus)+1) * WeaponItem.Damage * WeaponItem.Speed);			
			}else if(WeaponItem.itemType == Item.ItemType.Melee){
				return (int)(((Level + Skills.melee)*( TraitManager.GetTrait(Traits[0]).meleeBonus + TraitManager.GetTrait(Traits[1]).meleeBonus) +1) * WeaponItem.Damage * WeaponItem.Speed);			
			}else if(WeaponItem.itemType == Item.ItemType.Ranged){
				return (int)(((Level + Skills.archery )*( TraitManager.GetTrait(Traits[0]).rangedBonus + TraitManager.GetTrait(Traits[1]).rangedBonus) + 1) * WeaponItem.Damage * WeaponItem.Speed);			
			}else return (int)(((Level + Skills.melee )*( TraitManager.GetTrait(Traits[0]).meleeBonus + TraitManager.GetTrait(Traits[1]).meleeBonus) + 1));			
		}
	}
	public int Moral{
		get{
			return (int)Mathf.Clamp ((int)(TraitManager.GetTrait(Traits[0]).moraleBonus + TraitManager.GetTrait(Traits[1]).moraleBonus) + CalculateMoral(),0,100);
		}
	}
	
	
	public float CalculateMoral(){
		return 0.5f;
	}
	
	
	public void CreateCharacter(){
		Level = 1;
		Traits = TraitManager.GetRandomTraitsTypes(2);
		Skills.CreateSkillset();
		Name = CharacterManager.GenerateCharacterName();
		
		
	}
	
	private string iThink(){
		string message="";
		List<LabelManager.LabelType> accounted = new List<LabelManager.LabelType>(); 
		foreach(Character c in CharacterManager.gameCharacters){
			foreach(LabelManager.LabelType l in c.Labels){
				
				Trait t1 = TraitManager.GetTrait(this.Traits[0]);
				Trait t2 = TraitManager.GetTrait(this.Traits[1]);
				
				if(System.Array.IndexOf(t1.hateSelfLabel, l)>-1 || System.Array.IndexOf(t2.hateSelfLabel, l) >-1 && !accounted.Contains(l)){
				   message += LabelManager.GetLabel(l).DontLikeSelfString()+"\n";	
				   accounted.Add(l);
				}
				if(System.Array.IndexOf(t1.hateOtherLabel, l)>-1 || System.Array.IndexOf(t2.hateOtherLabel, l) >-1 && !accounted.Contains(l)){
					message += LabelManager.GetLabel(l).DontLikeOtherString("")+"\n";
					accounted.Add(l);	
				}
				
				
			}
		}
		return message;
	}
}
