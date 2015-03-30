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
	

}
