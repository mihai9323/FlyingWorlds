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
}