using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CharacterFightController : MonoBehaviour {

	[SerializeField] Portrait portrait;
	[SerializeField] Text t_Health, t_Morale, t_Damage;
	private Character character;

	public void LoadCharacterIn(Character character){
		if (character != null) {
			this.gameObject.SetActive (true);
			this.character = character;
			portrait.character = character;
			StopAllCoroutines ();
			UpdateStats ();
			StartCoroutine (RefreshStats ());
		} else {
			Debug.Log("Character is null");
			this.gameObject.SetActive (false);
		}
	}

	private void UpdateStats(){
		t_Damage.text = "Damage:"+character.Damage.ToString();
		t_Morale.text = "Morale:"+character.Moral.ToString();
		t_Health.text = "Health:"+character.Health.ToString();
	}
	private IEnumerator RefreshStats(){
		while (enabled && character!=null) {
			UpdateStats();
			yield return new WaitForSeconds(.5f);
		}
	}
}
