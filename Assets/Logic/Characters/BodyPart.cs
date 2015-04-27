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

	public void BuildRandomColor(){
		myColor = new Color (Random.value, Random.value, Random.value, 1.0f);
		this.GetComponent<SpriteRenderer> ().color = myColor;
	}
	public void BuildColor(Color color){
		this.GetComponent<SpriteRenderer> ().color = color;
		myColor = color;
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
