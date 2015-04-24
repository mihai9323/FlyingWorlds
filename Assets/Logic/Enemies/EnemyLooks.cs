using UnityEngine;
using System.Collections;

public class EnemyLooks : MonoBehaviour {

	[SerializeField] BodyPart a_weapon, a_body, a_head;
	private Color bodyColor;

	public void GenerateLooks(){
		a_body.BuildRandomColor();
		bodyColor = a_body.GetComponent<SpriteRenderer> ().color;
	}
	public void SetActiveWeapon(Item item){
		a_weapon.BuildColor(item.color);
	}
	public void RemoveActiveWeapon(){
		a_weapon.gameObject.SetActive (false);
	}
	public void SetActiveArmor(Item item){
			if (item.itemType == Item.ItemType.Armor) {
				a_body.BuildColor (item.color);
			} else
				a_body.BuildColor (bodyColor);
	}
	public void StartAnimation(string animation){
			if(a_body!=null)a_body.StartAnimation (animation);
			if(a_head!=null)a_head.StartAnimation (animation);
			if(a_weapon!=null)a_weapon.StartAnimation (animation);
	}
	public void StopAnimation(){
			if(a_body!=null)a_body.StopAnimation ();
			if(a_head!=null)a_head.StopAnimation ();
			if(a_weapon!=null)a_weapon.StopAnimation ();
		}
	public void HideAll(){
		a_body.gameObject.SetActive (false);
		a_weapon.gameObject.SetActive (false);
		a_head.gameObject.SetActive (false);
	}

}
