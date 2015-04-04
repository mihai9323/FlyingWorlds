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
	
	public float Damage;
	public float Defence;
	public float Speed;
	public ItemType itemType;
	public Character itemOwner;
	
	public Item(){
		itemType = ItemType.None;
		Damage =0;
		Defence = 0;
		Speed = 0;
	}
	public Item(int dmg, int def, int speed){
		ItemType iType;
		float rNr = Random.value;
		if(rNr<0.4f) iType = ItemType.Armor;
		else if(rNr<0.6f){ iType = ItemType.Melee; def = 0; }
		else if(rNr<.8f){ iType = ItemType.Ranged; def = 0; }
		else if(rNr<1f){ iType = ItemType.Magic; def = 0; }
		else iType = ItemType.None;
		
		this.Damage = dmg;
		this.Defence = def;
		this.Speed = speed;
		this.itemType = iType;
		
	}
	public Item(int dmg, int def, int speed, ItemType type){
		this.Damage = dmg;
		this.Defence = def;
		this.Speed = speed;
		this.itemType = type;
	}
	public Sprite GetSprite(out Color color){
		color = CalculateColor();

		switch(itemType){
			case ItemType.None: return null; break;
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
