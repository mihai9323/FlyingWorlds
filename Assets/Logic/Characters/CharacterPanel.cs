using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour {
	
	
	[SerializeField] ItemOnCharacter m_weapon_item, m_armor_item;
	
	[SerializeField] Text m_name;
	[SerializeField] Text m_health;
	[SerializeField] Text m_armor;
	[SerializeField] Text m_dmg;
	[SerializeField] Text m_morale;
	[SerializeField] Text m_trait1;
	[SerializeField] Text m_trait2;
	[SerializeField] Text m_melee;
	[SerializeField] Text m_archery;
	[SerializeField] Text m_magic;
	[SerializeField] Text m_level;
	[SerializeField] Text m_thinks;
	
	public void SetInfo(Character character){
	
		m_weapon_item.DisplayItem(character.WeaponItem);
		m_armor_item.DisplayItem(character.ArmorItem);
	
		m_name.text = character.Name;
		m_health.text = "Heath:"+character.MaxHealth.ToString()+"/"+ character.MaxHealth.ToString();
		m_armor.text = "Armor:"+character.Armor.ToString();
		m_dmg.text = "Damage:"+character.Damage.ToString();
		m_morale.text = "Morale:"+ character.Moral.ToString()+"%";
		m_trait1.text = TraitManager.GetTrait(character.Traits[0]).name;
		m_trait2.text = TraitManager.GetTrait(character.Traits[1]).name;
		m_melee.text = "Melee: "+character.Skills.melee;
		m_archery.text = "Archery: "+character.Skills.archery;
		m_magic.text = "Magic: "+ character.Skills.magic;
		m_level.text = "Level: "+character.Level;
		m_thinks.text = character.Thinks;
	}
}
