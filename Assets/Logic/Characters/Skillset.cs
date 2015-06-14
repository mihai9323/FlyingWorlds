using UnityEngine;
using System.Collections;
[System.Serializable]
public class Skillset  {

	public float melee;
	public float archery;
	public float magic;
	public float health;
	
	public void CreateSkillset(){
		melee = Random.Range(0,5);
		archery = Random.Range (0,5);
		magic = Random.Range(0,5);
	}
	public void CreateOneSkillset(){
		melee = archery = magic = 1;
	}
	public void CreateSkillset(bool turnBased){
		Debug.Log ("<color=blue>Progression: " + GameData.Progression + "</color>");
		int min = GameData.Progression/2;
		int max = GameData.Progression;
		melee = Random.Range(min,max) + melee;
		archery = Random.Range (min,max) + archery;
		magic = Random.Range(min,max) + magic;
		health = Random.Range (min, max) * ((float)health/5f) + health; 

	}
}
