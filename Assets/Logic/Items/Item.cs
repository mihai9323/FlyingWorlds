using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Item  {

	public enum ItemType{
		None,
		Armor,
		Melee,
		Ranged,
		Magic
		
	}
	public string ItemName{
		get{
			if(string.IsNullOrEmpty(item_name)){
				return itemType == ItemType.Armor ? "Armor" :
					   itemType == ItemType.Melee ? "Sword" :
					   itemType == ItemType.Ranged? "Bow" :
					   itemType == ItemType.Magic ? "Staff" : "None";
								  
			}else return item_name;
		}
		set{
			item_name = value;
		}
	}
	public Dictionary<MonsterTypes, int> monstersKilled;
	public string fightAnimation{
		get{

				return
			   itemType == ItemType.Melee ? AnimationNames.kSwordAttack :
			   itemType == ItemType.Ranged? AnimationNames.kBowAttack :
			   itemType == ItemType.Magic ? AnimationNames.kMagicAttack : "";
				

		}
	}

	private string item_name;
	public float Damage;
	public float Defence;
	public float Range;
	public int shine;
	public ItemType itemType;
	public Character itemOwner;
	public int Value{
		get{
			return (int)(40 * (Damage + 2*Defence + 2*Range) * (1+shine) + ItemName.Length * 20);
		}
	}
	public string MostMonstersKilled{
		get{
			KeyValuePair<MonsterTypes,int> bestPair;
			bestPair = new KeyValuePair<MonsterTypes, int>(MonsterTypes.Devil,0);
			foreach(KeyValuePair<MonsterTypes,int> pair in monstersKilled){
				if(pair.Value>bestPair.Value){
					bestPair = pair;
				}
			}
			return bestPair.Key.ToString();
		}
	}
	public string colorName;
	public Color color{
		get{
			Color c;
			GetSprite(out c);
			return c;
		}
	}
	public Item(){
		itemType = ItemType.None;
		Damage =0;
		Defence = 0;
		Range = 0;
		monstersKilled = new Dictionary<MonsterTypes,int>();
	}
	public Item(Item item, float power){
		this.itemType = item.itemType;
		this.Range = item.Range * power;
		this.Damage = item.Damage * power;
		this.Defence = item.Defence * power;
		this.ItemName = item.ItemName;
	}
	public Item(ItemType type, int power){
		this.itemType = type;
		switch (type) {
		case ItemType.Magic: this.Range = 2 + power * Random.value; this.Defence = 0; this.Damage = 1+ power * Random.value*2; break;
		case ItemType.Melee: this.Range = 1; this.Defence = 1+ power * Random.value; this.Damage = 3+ power * Random.value*3; break;
		case ItemType.Ranged: this.Range = 3 + power * Random.value; this.Defence = 0; this.Damage = 1+ power * Random.value; break;

		}
		monstersKilled = new Dictionary<MonsterTypes,int>();
	}
	public Item(int dmg, int def, int range){
		ItemType iType;
		float rNr = Random.value;
		if (rNr < 0.4f) { iType = ItemType.Armor; dmg = 0; range = 0;}
		else if(rNr<0.6f){ iType = ItemType.Melee; def = (int)((float)def * .3f); range = Mathf.Clamp(range,0,2); }
		else if(rNr<.8f){ iType = ItemType.Ranged; def = 0; range = Mathf.Clamp(range,6,20);}
		else if(rNr<1f){ iType = ItemType.Magic; def = 0; range = Mathf.Clamp(range,6,20);}
		else iType = ItemType.None;
		
		this.Damage = dmg;
		this.Defence = def;
		this.Range = range;
		this.itemType = iType;
		monstersKilled = new Dictionary<MonsterTypes,int>();
	}
	public Item(ItemType iType,int dmg, int def, int range){
		
		
		if (iType == ItemType.Armor) {  dmg = 0; range = 0;}
		else if(iType == ItemType.Melee){  def = (int)((float)def * .3f); range = Mathf.Clamp(range,0,2); }
		else if(iType == ItemType.Ranged){  def = 0; range = Mathf.Clamp(range,6,20);}
		else if(iType == ItemType.Magic){  def = 0; range = Mathf.Clamp(range,6,20);}
		
		
		this.Damage = dmg;
		this.Defence = def;
		this.Range = range;
		this.itemType = iType;

		monstersKilled = new Dictionary<MonsterTypes,int>();
	}
	public Item(ItemType iType,int dmg, int def, int range, string name){


		if (iType == ItemType.Armor) {  dmg = 0; range = 0;}
		else if(iType == ItemType.Melee){  def = (int)((float)def * .3f); range = Mathf.Clamp(range,0,2); }
		else if(iType == ItemType.Ranged){  def = 0; range = Mathf.Clamp(range,6,20);}
		else if(iType == ItemType.Magic){  def = 0; range = Mathf.Clamp(range,6,20);}

		
		this.Damage = dmg;
		this.Defence = def;
		this.Range = range;
		this.itemType = iType;
		this.ItemName = name+" "+ this.ItemName;
		monstersKilled = new Dictionary<MonsterTypes,int>();
	}


	public Sprite GetSprite(out Color color){
		color = CalculateColor();
		switch(itemType){
			case ItemType.None: return null; color = new Color(0,0,0,0); break;
		    case ItemType.Armor: return SpriteManager.ArmorSprite; break;
		    case ItemType.Melee: return SpriteManager.SwordSprite; break;
		    case ItemType.Ranged: return SpriteManager.BowSprite; break;
		    case ItemType.Magic: return SpriteManager.StaffSprtie; break;
		    default: return null;
		}
	}
	
	private Color CalculateColor(){
		if (Damage + Defence < 5) {
			colorName = "Gray";
			return Color.gray;
		} else if (Damage + Defence < 10) {
			colorName = "White";
			return Color.white;
		} else if (Damage + Defence < 18) {
			colorName = "Green";
			return Color.green;
		} else if (Damage + Defence < 25) {
			colorName = "Yellow";
			return Color.yellow;
		} else if (Damage + Defence < 35) {
			colorName = "Cyan";
			return Color.cyan;
		}else if (Damage + Defence < 42) {
			colorName = "Blue";
			return Color.blue;
		}else if (Damage + Defence < 50) {
			colorName = "Red";
			return Color.red;
		}else if (Damage + Defence < 60) {
			colorName = "Magenta";
			return Color.magenta;
		} else {
			colorName = "Black";
			return Color.black;
		}

	}

}
