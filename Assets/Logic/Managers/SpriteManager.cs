using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour {

	[SerializeField] Sprite swordSprite, bowSprte, staffSprite, armorSprite;
	public static Sprite SwordSprite, BowSprite, StaffSprtie, ArmorSprite;
	
	private void Awake(){
		SwordSprite = swordSprite;
		BowSprite = bowSprte;
		StaffSprtie = staffSprite;
		ArmorSprite = armorSprite;
	}
}
