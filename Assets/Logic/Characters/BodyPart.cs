using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyPart : MonoBehaviour {

	public Color myColor;
	private Animator animator;
	private string currentAnimation;

	private SpriteRenderer sRenderer;

	 
	public Sprite mySprite{
		get {
			return sRenderer.sprite;
		}
	}

	private void Awake(){
		sRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
		this.gameObject.SetActive (false);
		this.animator = this.gameObject.GetComponent<Animator> ();



	}
	public void SetLightColor(Color color){
		sRenderer.material.SetColor ("_LightColor", color);
	}
	public void BuildRandomColor(){
		myColor = new Color (Random.value, Random.value, Random.value, 1.0f);
		this.GetComponent<SpriteRenderer> ().color = myColor;
	}
	public void BuildColor(Color color){
		this.GetComponent<SpriteRenderer> ().color = color;
		myColor = color;
	}
	public void BuildAndSetRandomColors(float[,] faceRandom){
		Color[] colors = new Color[6];
		Color dayColor = FightManager.battles [GameData.nextBattleID].dayColor;
		for (int i =0; i<6; i++) {
			if(i<3)colors[i] = new Color(Random.Range(faceRandom[0,0],faceRandom[0,1])*dayColor.r,Random.Range(faceRandom[1,0],faceRandom[1,1])*dayColor.g,Random.Range(faceRandom[2,0],faceRandom[2,1])*dayColor.b);
			else colors[i] = new Color(Random.value*dayColor.r,Random.value*dayColor.g,Random.value*dayColor.b); 
		}
		SetColors (colors);
	}
	public void SetColors(Color[] colors){
		Material mat = new Material (SpriteManager.monsterShader);
		mat.SetColor ("_Color", ColorCodes.tint);
		for (int i =0; i<6; i++) {
			mat.SetColor("_Color"+(i+1).ToString(),colors[i]);
		}
		sRenderer.material = mat;
	}

	public void StartAnimation(string animation){
		if (!string.IsNullOrEmpty (currentAnimation)) {
			StopAnimation();
		}
		if(!string.IsNullOrEmpty(animation) && animator !=null)animator.SetBool (HashIDs.AnimationStringToID(animation), true);
		currentAnimation = animation;
		//StartCoroutine ("Animate");
	}
	public void StopAnimation(){
		if(!string.IsNullOrEmpty(currentAnimation))animator.SetBool (HashIDs.AnimationStringToID(currentAnimation), false);

		//StopCoroutine ("Animate");
	}



}
