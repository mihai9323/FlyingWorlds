using UnityEngine;
using System.Collections;

public class CharacterLooks : MonoBehaviour {

	[SerializeField] BodyPart[] body,beards,eyes,eyebrows,hair,frontArms,mouthes,noses,weapons;




	[HideInInspector]public BodyPart a_body,a_beard,a_eyes,a_eyebrows,a_hair,a_frontArm,a_mouthes,a_noses,a_weapon;
	private Color bodyColor;
	public void GenerateLooks(){
		(a_body = getAndActivateBodyPart (body)).BuildRandomColor();
		(a_beard = getAndActivateBodyPart (beards)).BuildRandomColor();
		(a_eyes = getAndActivateBodyPart (eyes)).BuildRandomColor();
		(a_eyebrows = getAndActivateBodyPart (eyebrows)).BuildRandomColor();
		(a_hair = getAndActivateBodyPart (hair)).BuildRandomColor();
		(a_frontArm = getAndActivateBodyPart (frontArms)).BuildRandomColor();
		(a_mouthes = getAndActivateBodyPart (mouthes)).BuildRandomColor();
		(a_noses = getAndActivateBodyPart (noses)).BuildRandomColor();
		bodyColor = a_body.GetComponent<SpriteRenderer> ().color;
	}
	public void SetActiveWeapon(Item item){
		RemoveActiveWeapon ();

		switch (item.itemType) {
		case Item.ItemType.Magic: a_weapon = weapons[0]; a_weapon.gameObject.SetActive(true); a_weapon.BuildColor(item.color); break;
		case Item.ItemType.Melee: a_weapon = weapons[1]; a_weapon.gameObject.SetActive(true); a_weapon.BuildColor(item.color); break;
		case Item.ItemType.Ranged: a_weapon = weapons[2]; a_weapon.gameObject.SetActive(true); a_weapon.BuildColor(item.color); break;
		case Item.ItemType.None: if(a_weapon!=null) {a_weapon.gameObject.SetActive(false); a_weapon.BuildColor(new Color(0,0,0,0));} break;
		}

	}
	public void RemoveActiveWeapon(){
		foreach (BodyPart weapon in weapons) {
			weapon.gameObject.SetActive(false);
			weapon.BuildColor(new Color(0,0,0,0));
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
	public void StartAnimation(string animation){
		if(a_body!=null)a_body.StartAnimation (animation);
		if(a_beard!=null)a_beard.StartAnimation (animation);
		if(a_eyes!=null)a_eyes.StartAnimation (animation);
		if(a_eyebrows!=null)a_eyebrows.StartAnimation (animation);
		if(a_hair!=null)a_hair.StartAnimation (animation);
		if(a_frontArm!=null)a_frontArm.StartAnimation (animation);
		if(a_mouthes!=null)a_mouthes.StartAnimation (animation);
		if(a_noses!=null)a_noses.StartAnimation (animation);
		if(a_weapon!=null)a_weapon.StartAnimation (animation);
	}
	public void StopAnimation(){
		if(a_body!=null)a_body.StopAnimation ();
		if(a_beard!=null)a_beard.StopAnimation ();
		if(a_eyes!=null)a_eyes.StopAnimation ();
		if(a_eyebrows!=null)a_eyebrows.StopAnimation ();
		if(a_hair!=null)a_hair.StopAnimation ();
		if(a_frontArm!=null)a_frontArm.StopAnimation ();
		if(a_mouthes!=null)a_mouthes.StopAnimation ();
		if(a_noses!=null)a_noses.StopAnimation ();
		if(a_weapon!=null)a_weapon.StopAnimation ();
	}

}