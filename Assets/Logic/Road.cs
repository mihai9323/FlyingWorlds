using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour {
	public Transform RoadWorldSpace;

	public void OnClick(){
		if ( !CharacterManager.partyEmpty) {
			CharacterManager.MoveAllActiveHere(RoadWorldSpace.position);
		} else
			Debug.Log ("Party empty");
	}
}
