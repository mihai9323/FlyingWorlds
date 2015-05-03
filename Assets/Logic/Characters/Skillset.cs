using UnityEngine;
using System.Collections;
[System.Serializable]
public class Skillset  {

	public float melee;
	public float archery;
	public float magic;

	
	public void CreateSkillset(){
		melee = Random.Range(0,5);
		archery = Random.Range (0,5);
		magic = Random.Range(0,5);
	}
	public void CreateSkillset(bool turnBased){
		int min = GameData.TurnNumber/2;
		int max = GameData.TurnNumber;
		melee = Random.Range(min,max);
		archery = Random.Range (min,max);
		magic = Random.Range(min,max);

	}
}
