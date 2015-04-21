using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	public Color myColor;
	public Sprite mySprite{
		get{
			return this.gameObject.GetComponent<SpriteRenderer>().sprite;
		}
	}
	private void Awake(){
		this.gameObject.SetActive (false);

	}

	public void BuildRandomColor(){
		myColor = new Color (Random.value, Random.value, Random.value, 1.0f);
		this.GetComponent<SpriteRenderer> ().color = myColor;
	}
	public void BuildColor(Color color){
		this.GetComponent<SpriteRenderer> ().color = color;
		myColor = color;
	}


}
