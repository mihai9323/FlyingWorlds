using UnityEngine;
using System.Collections;
[System.Serializable]
public class Battle {

	public string id;
	public string location;
	public string moment;
	public GameObject fightBackground;
	public float[] monsterSpawnChances;
	public Color dayColor;

	public void Generate(){
		this.fightBackground.SetActive (true);
		fightBackground.renderer.material.color = dayColor;
		foreach(SpriteRenderer sr in fightBackground.transform.GetComponentsInChildren<SpriteRenderer>()){
			sr.color = dayColor;
		}
	}
	public Battle(string id, string location, string moment, GameObject fightBackground, Color dayColor, float[] monsterSpawnChances){
		this.id = id;
		this.location = location;
		this.moment = moment;
		this.fightBackground = fightBackground;
		this.dayColor = dayColor;
		this.monsterSpawnChances = monsterSpawnChances;
	}
}