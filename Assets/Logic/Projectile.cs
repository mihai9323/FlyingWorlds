using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Projectile : MonoBehaviour {
	[SerializeField]private float movementSpeed = 6;
	[SerializeField]private float arrowHeight = 3;
	[SerializeField]private float areaOfAttack = 1;
	[SerializeField]private bool rotate = true;


	public void ShootCharacter(Vector3 targetPos, int damage, VOID_FUNCTION_CHARACTER OnCharacterResponse){
		StartCoroutine (WaitForResponse (targetPos, damage, OnCharacterResponse));
	}
	public void ShootMonster(Vector3 targetMonster,int damage, VOID_FUNCTION_ENEMY OnEnemyResponse){
		StartCoroutine (WaitForResponse (targetMonster, damage, OnEnemyResponse));
	}

	private IEnumerator	Move(Vector3 targetPos){
		Vector3 lScale = transform.localScale;
		lScale.x *= (int)Mathf.Sign (targetPos.x - transform.position.x);
		transform.localScale = lScale;
		float initialDistance = Vector2.Distance(this.transform.position,targetPos);
		Vector2 initialPosition = transform.position;
		float ct = 0;
		arrowHeight = arrowHeight * initialDistance / 30f;
		while (ct<1) {
			yield return null;
			float initZ = transform.position.z;
			ct += movementSpeed * Time.deltaTime /initialDistance;
			Vector3 prevPos = transform.position;
			this.transform.position = Vector2.Lerp(initialPosition, 
			                                       targetPos, 
			                                       ct
			                                       );

			initZ = Mathf.Lerp(-10,0,Camera.main.WorldToScreenPoint(transform.position).y/Screen.height);
			transform.position = new Vector3(transform.position.x,transform.position.y+arrowHeight * Mathf.Sin(Mathf.PI*ct),initZ);
			if(rotate){
				float xMovement = transform.position.x-prevPos.x;
				transform.rotation = Quaternion.Euler(0,0,
			                                      Mathf.Tan(
														(transform.position.y-prevPos.y) /(Mathf.Sign(xMovement)*Mathf.Max(0.01f,Mathf.Abs(xMovement))		  										)
													)*180f/Mathf.PI);

			}
		}
	}

	private IEnumerator	WaitForResponse(Vector3 targetPos,int damage, VOID_FUNCTION_CHARACTER OnCharacterResponse){
		yield return StartCoroutine(Move(targetPos));
		Character c = FindCharacterTarget();
		if (c != null) {
			c.Hit (damage);
		}
		Destroy (this.gameObject, .2f);
		if(OnCharacterResponse!=null)OnCharacterResponse(c);

	}
	private IEnumerator WaitForResponse(Vector3 targetPos,int damage, VOID_FUNCTION_ENEMY OnEnemyResponse){
		yield return StartCoroutine(Move(targetPos));
		Enemy e = FindEnemyTarget ();
		if(e!=null)e.Hit (damage);
		Destroy (this.gameObject, .2f);
		if(OnEnemyResponse!=null)OnEnemyResponse (e);
	}


	private Enemy FindEnemyTarget ()
	{
		Enemy targetCharacter = null;
		GameObject gob = GameObject.FindGameObjectsWithTag ("ENEMY").OrderBy (go => Vector3.SqrMagnitude (go.transform.position - transform.position)).FirstOrDefault ();
		if (gob != null && gob.GetComponent<Enemy> () != null && Vector3.Distance(gob.transform.position,this.transform.position)<areaOfAttack)
			targetCharacter = gob.GetComponent<Enemy> ();
		return targetCharacter;
	}
	private Character FindCharacterTarget ()
	{
		Character targetCharacter = null;
		GameObject gob = GameObject.FindGameObjectsWithTag ("CHARACTER").OrderBy (go => Vector3.SqrMagnitude (go.transform.position - transform.position)).FirstOrDefault ();
		if (gob != null && gob.GetComponent<Character> () != null && Vector3.Distance(gob.transform.position,this.transform.position)<areaOfAttack)
			targetCharacter = gob.GetComponent<Character> ();
		return targetCharacter;
	}
}
