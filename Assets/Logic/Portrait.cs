using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Portrait : MonoBehaviour {
	
	public Character character;
	[SerializeField] Image iBody,iBeard,iEyes,iEyeBrow,iHair,iFrontArm,iMouth,iNose,iWeapon;
	bool waited;
	bool activeProfile;
	[SerializeField] HealthBar healthBar;
	[SerializeField] MoraleDisplay moraleDisplay;
	[SerializeField] Text characterNameText;
	[SerializeField] Image trait1Image, trait2Image;
	private IEnumerator LoadCoroutine(){
		yield return null;
		if (character) {
			while (!character.Ready) {
				yield return null;
			}
			while(character.Ready){
				yield return new WaitForSeconds(1f/12);
				LoadCharacter ();
			}
		}
	}
	private void OnEnable(){
		if (character != null && character.Ready) {
			if(healthBar!=null)healthBar.UpdateStatus(character.MaxHealth,character.Health);
			if(moraleDisplay!=null)moraleDisplay.DisplayPortrait(character.CalculateMoral());
			if(trait1Image!=null && character.trait0.imageSprite!=null) trait1Image.sprite = character.trait0.imageSprite; 
			if(trait2Image!=null && character.trait1.imageSprite!=null) trait2Image.sprite = character.trait1.imageSprite;
		}

		StartCoroutine ("LoadCoroutine");
	}
	private void Start(){
		if (this.transform.parent.name == "Grid") {
			this.GetComponent<Image> ().color = Color.gray;
			activeProfile = false;
		}
		if (characterNameText != null) {
			characterNameText.text = character.Name;
		}
	}
	public void OpenPortrait(){
		HubManager.ShowCharacter(character);
	}
	public void LoadCharacter(){

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
		if (characterNameText != null) {
			characterNameText.text = character.Name;
		}
		if(trait1Image!=null && character.trait0.imageSprite!=null) trait1Image.sprite = character.trait0.imageSprite; 
		if(trait2Image!=null && character.trait1.imageSprite!=null) trait2Image.sprite = character.trait1.imageSprite;

	}
	private void Update(){
		if (enabled) {
			if(this.transform.parent.name == "Grid"){
				if(this.character == CharacterManager.SelectedCharacter){
					if(!activeProfile){
						activeProfile = true;
						this.GetComponent<Image>().color = Color.white;
					}
				}else{
					if(activeProfile){
						activeProfile = false;
						this.GetComponent<Image>().color = Color.gray;
					}
				}
			}
		}
	}


	public void OpenBarracks(){
		if (HubManager.interactable) {
			HubManager.HideAll ();
			HubManager.ShowCharacters (character);
		}
	}
}
