using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	private void Start(){
		BuildRandomColor ();
	}
	private void Awake(){
		this.gameObject.SetActive (false);
	}
	public void BuildRandomColor(){
		this.GetComponent<SpriteRenderer> ().color = new Color (Random.value, Random.value, Random.value, 1.0f);
	}
	public void BuildColor(Color color){
		this.GetComponent<SpriteRenderer> ().color = color;
	}


}
