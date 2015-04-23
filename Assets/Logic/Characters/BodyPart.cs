using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	public Color myColor;
	private Animator animator;
	private string currentAnimation;
	public Sprite mySprite{
		get{
			return this.gameObject.GetComponent<SpriteRenderer>().sprite;
		}
	}
	private void Awake(){
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
		animator.SetBool (animation, true);
		currentAnimation = animation;
	}
	public void StopAnimation(){
		animator.SetBool (currentAnimation, false);
		currentAnimation = "";
	}



}
