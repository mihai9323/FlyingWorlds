using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {
	[SerializeField] Text T_HealthValue;
	[SerializeField] RectTransform _healthBar;
	Vector2 initSizeDelta;
	Vector3 initPos;
	bool InitSet;


	public void UpdateStatus(float maxHealth, float remainingHealth){
		if (!InitSet) {
			InitSet = true;
			initSizeDelta = _healthBar.sizeDelta;
			initPos = _healthBar.localPosition;
		}
		_healthBar.sizeDelta = new Vector2 ((remainingHealth / maxHealth * initSizeDelta.x),initSizeDelta.y);
		float SizeDifference = initSizeDelta.x - (remainingHealth / maxHealth * initSizeDelta.x);

		_healthBar.localPosition = new Vector3 (initPos.x - SizeDifference/2, initPos.y, initPos.z);
		T_HealthValue.text = remainingHealth + "/" + maxHealth;
	
	}

	private void OnDisable(){
		//InitSet = false;
	}
}
