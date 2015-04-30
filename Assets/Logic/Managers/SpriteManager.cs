using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour {
	[SerializeField] Shader _monsterShader;
	[SerializeField] Sprite swordSprite, bowSprte, staffSprite, armorSprite;
	public static Sprite SwordSprite, BowSprite, StaffSprtie, ArmorSprite;
	public static Shader monsterShader;
	private void Awake(){
		SwordSprite = swordSprite;
		BowSprite = bowSprte;
		StaffSprtie = staffSprite;
		ArmorSprite = armorSprite;
		monsterShader = _monsterShader;
	}
}
