using UnityEngine;
using System.Collections;

public class CharacterLooks : MonoBehaviour {

	[SerializeField] BodyPart[] body,beards,eyes,eyebrows,hair,frontArms,mouthes,noses,weapons;

	[HideInInspector]public BodyPart a_body,a_beard,a_eyes,a_eyebrows,a_hair,a_frontArm,a_mouthes,a_noses,a_weapon;
	private Color bodyColor;
	public void GenerateLooks(){
		a_body = getAndActivateBodyPart (body);
		a_beard = getAndActivateBodyPart (beards);
		a_eyes = getAndActivateBodyPart (eyes);
		a_eyebrows = getAndActivateBodyPart (eyebrows);
		a_hair = getAndActivateBodyPart (hair);
		a_frontArm = getAndActivateBodyPart (frontArms);
		a_mouthes = getAndActivateBodyPart (mouthes);
		a_noses = getAndActivateBodyPart (noses);
		bodyColor = a_body.GetComponent<SpriteRenderer> ().color;
	}
	public void SetActiveWeapon(Item item){
		switch (item.itemType) {
		case Item.ItemType.Magic: a_weapon = weapons[0]; a_weapon.gameObject.SetActive(true); a_weapon.BuildColor(item.color); break;
		case Item.ItemType.Melee: a_weapon = weapons[0]; a_weapon.gameObject.SetActive(true); a_weapon.BuildColor(item.color); break;
		case Item.ItemType.Ranged: a_weapon = weapons[0]; a_weapon.gameObject.SetActive(true); a_weapon.BuildColor(item.color); break;
		case Item.ItemType.None: if(a_weapon!=null) a_weapon.gameObject.SetActive(false); break;
		}
	}
	public void SetActiveArmor(Item item){
		if (item.itemType == Item.ItemType.Armor) {
			a_body.BuildColor (item.color);
		} else
			a_body.BuildColor (bodyColor);
	}

	private BodyPart getRandomBodyPart(BodyPart[] bodyPartsArray){
		if (bodyPartsArray != null) {
			return bodyPartsArray[(int)Mathf.Clamp(Random.Range(0,bodyPartsArray.Length),0,bodyPartsArray.Length-1)];
		}else return null;
	}
	private BodyPart getAndActivateBodyPart(BodyPart[] bodyPartsArray){
		BodyPart bp = getRandomBodyPart (bodyPartsArray);
		if (bp != null) {
			bp.gameObject.SetActive(true);
		}
		return bp;
	}
}