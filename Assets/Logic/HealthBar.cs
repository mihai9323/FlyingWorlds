using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour {
	[SerializeField] Text T_HealthValue;
	[SerializeField] RectTransform _healthBar;
	Vector2 initSizeDelta;
	private void Start(){
		initSizeDelta = _healthBar.sizeDelta;
	}

	public void UpdateStatus(float maxHealth, float remainingHealth){
		_healthBar.sizeDelta = new Vector2 ((remainingHealth / maxHealth * initSizeDelta.x),initSizeDelta.y);
		T_HealthValue.text = remainingHealth + "/" + maxHealth;
	}
}
