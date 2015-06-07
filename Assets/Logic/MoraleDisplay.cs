using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoraleDisplay : MonoBehaviour {
	[SerializeField] Image moraleImage;
	[SerializeField] Sprite[] moraleSprites;

	public void DisplayPortrait(float moraleValue){
		if (moraleSprites != null) {
			int max = moraleSprites.Length;
			int value = (int)(moraleValue * (float)max);
			value = Mathf.Clamp (value, 0, moraleSprites.Length-1);
			moraleImage.sprite = moraleSprites [value];
		}
	}
}
