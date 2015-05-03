using UnityEngine;
using System.Collections;
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
			return (int)(50 * (Damage + 3*Defence + Range) * (1+shine) + ItemName.Length * 20);
		}
	}
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
	}
	public Item(ItemType type, int power){
		this.itemType = type;
		switch (type) {
		case ItemType.Magic: this.Range = 2 + power * Random.value; this.Defence = 0; this.Damage = 1+ power * Random.value*2; break;
		case ItemType.Melee: this.Range = 1; this.Defence = 0; this.Damage = 3+ power * Random.value*3; break;
		case ItemType.Ranged: this.Range = 3 + power * Random.value; this.Defence = 0; this.Damage = 1+ power * Random.value; break;

		}
	}
	public Item(int dmg, int def, int range){
		ItemType iType;
		float rNr = Random.value;
		if (rNr < 0.4f) { iType = ItemType.Armor; dmg = 0; range = 0;}
		else if(rNr<0.6f){ iType = ItemType.Melee; def = 0; range = Mathf.Clamp(range,0,2); }
		else if(rNr<.8f){ iType = ItemType.Ranged; def = 0; range = Mathf.Clamp(range,6,20);}
		else if(rNr<1f){ iType = ItemType.Magic; def = 0; range = Mathf.Clamp(range,6,20);}
		else iType = ItemType.None;
		
		this.Damage = dmg;
		this.Defence = def;
		this.Range = range;
		this.itemType = iType;
		
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
		if(Damage<10) return Color.white;
		else if(Damage<20) return Color.green;
		else if(Damage<30) return Color.red;
		else if(Damage<40) return Color.blue;
		else return Color.black;
	}

}
