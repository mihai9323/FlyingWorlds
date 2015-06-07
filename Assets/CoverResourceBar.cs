using UnityEngine;
using System.Collections;

public class CoverResourceBar : MonoBehaviour {

	[SerializeField] Transform t;
	// Update is called once per frame
	void Update () {
		if (HubManager.interactable) {
			t.gameObject.SetActive (false);
		} else
			t.gameObject.SetActive (true);
	}
}
