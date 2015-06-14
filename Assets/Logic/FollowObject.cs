using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {
	[SerializeField]Vector3 distance = new Vector3(0,2,0);
	[SerializeField]Transform targetObject;
	public HealthBar healthBar;
	bool startedFollowing = false;
	public void StartFollow(Transform t){
		startedFollowing = true;
		targetObject = t;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(startedFollowing)this.transform.position = targetObject.transform.position + distance;
	}
}
