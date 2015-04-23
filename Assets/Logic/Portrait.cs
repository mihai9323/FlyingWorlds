using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Portrait : MonoBehaviour {
	
	public Character character;
	[SerializeField] Image iBody,iBeard,iEyes,iEyeBrow,iHair,iFrontArm,iMouth,iNose,iWeapon;
	bool waited;

	private IEnumerator LoadCoroutine(){
		while (!character.Ready) {
			yield return null;
		}
		LoadCharacter ();
	}
	private void OnEnable(){
		StartCoroutine ("LoadCoroutine");
	}
	public void OpenPortrait(){
		HubManager.ShowCharacter(character);
	}
	public void LoadCharacter(){
		Debug.Log (character.Name);
		waited = true;
		iBody.sprite = character.Looks.a_body.mySprite;
		iBody.color = character.Looks.a_body.myColor;

		iBeard.sprite = character.Looks.a_beard.mySprite;
		iBeard.color = character.Looks.a_beard.myColor;

		iEyes.sprite = character.Looks.a_eyes.mySprite;
		iEyes.color = character.Looks.a_eyes.myColor;

		iEyeBrow.sprite = character.Looks.a_eyebrows.mySprite;
		iEyeBrow.color = character.Looks.a_eyebrows.myColor;

		iHair.sprite = character.Looks.a_hair.mySprite;
		iHair.color = character.Looks.a_hair.myColor;

		iFrontArm.sprite = character.Looks.a_frontArm.mySprite;
		iFrontArm.color = character.Looks.a_frontArm.myColor;

		iMouth.sprite = character.Looks.a_mouthes.mySprite;
		iMouth.color = character.Looks.a_mouthes.myColor;

		iNose.sprite = character.Looks.a_noses.mySprite;
		iNose.color = character.Looks.a_noses.myColor;
		if (character.Looks.a_weapon != null) {
			iWeapon.sprite = character.Looks.a_weapon.mySprite;
			iWeapon.color = character.Looks.a_weapon.myColor;
		} else
			iWeapon.color = new Color (0, 0, 0, 0);

	}
}
