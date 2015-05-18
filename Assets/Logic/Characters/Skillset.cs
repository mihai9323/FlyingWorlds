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
	public void CreateSkillset(bool turnBased){
		int min = GameData.TurnNumber/2;
		int max = GameData.TurnNumber;
		melee = Random.Range(min,max) + melee;
		archery = Random.Range (min,max) + archery;
		magic = Random.Range(min,max) + magic;
		health = Random.Range (min, max) + health; 

	}
}
