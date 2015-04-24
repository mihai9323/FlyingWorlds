using UnityEngine;
using System.Collections;

public class DetectEnemy : MonoBehaviour {
	private bool hitCharacter, hitEnemy;
	private Character characterHit;
	private Enemy enemyHit;
	public void StartDetection(VOID_FUNCTION_CHARACTER callback){
		hitCharacter = false;
		StartCoroutine (Grow (callback));

	}
	public void StartDetection(VOID_FUNCTION_ENEMY callback){
		hitEnemy = false;
		StartCoroutine (Grow (callback));
	}

	private IEnumerator Grow(VOID_FUNCTION_CHARACTER callback){
		float ct = 0;
		transform.localScale = Vector3.zero;
		while (ct<1f) {
			ct+=Time.deltaTime*2;
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 120f, ct);
			if(hitCharacter){

				callback(characterHit);
				break;
			}
			yield return null;
		}
		if (!hitCharacter)
			callback (null);
		transform.localScale = Vector3.zero;
		hitCharacter = false;
	}
	private IEnumerator Grow(VOID_FUNCTION_ENEMY callback){
		float ct = 0;
		transform.localScale = Vector3.zero;
		while (ct<1f) {
			ct+=Time.deltaTime/5;
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 120, ct);
			if(hitEnemy){

				callback(enemyHit);
				break;
			}
			yield return null;
		}
		if(!hitEnemy){

			callback(null);

		}
		transform.localScale = Vector3.zero;
		hitEnemy = false;
	}
	private void OnTriggerEnter(Collider col){
		if (col.tag == "ENEMY") {
			enemyHit = col.GetComponent<Enemy>();
			hitEnemy = true;
		}
		if (col.tag == "CHARACTER") {
			characterHit = col.GetComponent<Character>();
			hitCharacter = true;
		}
	}
}