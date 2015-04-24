using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public EnemyLooks looks;
	public Skillset skillset;
	public Item weapon;

	public int Damage{
		get{
			switch(weapon.itemType){
			case Item.ItemType.Magic: return (int)(weapon.Damage * skillset.magic); break;
			case Item.ItemType.Melee: return (int)(weapon.Damage * skillset.melee); break;
			case Item.ItemType.Ranged: return (int)(weapon.Damage * skillset.archery); break;
			}
			return 0;
		}
	}
	public int MaxHealth{
		get{
			return (int)((skillset.magic + skillset.melee + skillset.archery) * GameData.TurnNumber); 
		}
	}
	private int currentHealth;
	public bool dead;

	public void Hit(int damage){
		currentHealth -= damage;
		if (currentHealth < 0) {
			dead = true;
			looks.HideAll();
		}
	}


	public void GenerateEnemy(){
		looks.GenerateLooks ();
		looks.SetActiveWeapon (weapon);
		skillset.CreateSkillset (true);
		currentHealth = MaxHealth;
	}
}
