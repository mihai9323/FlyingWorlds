using UnityEngine;
using System.Collections;

public class EnemyLooks : MonoBehaviour {

	[SerializeField] BodyPart  a_head;


	public void GenerateLooks(MonsterTypes monsterType){
		a_head.BuildAndSetRandomColors (ColorCodes.getColorConfig(monsterType));
		ShowAll ();

	}
	public void SetActiveWeapon(Item item){

	}
	public void RemoveActiveWeapon(){

	}
	public void SetActiveArmor(Item item){
			
	}
	public void StartAnimation(string animation){
			
			if(a_head!=null)a_head.StartAnimation (animation);
			
	}
	public void StopAnimation(){
	
			if(a_head!=null)a_head.StopAnimation ();
			
		}
	public void HideAll(){
		this.gameObject.SetActive (false);

		a_head.gameObject.SetActive (false);

	}
	public void ShowAll(){
	
		a_head.gameObject.SetActive (true);

	}


}
