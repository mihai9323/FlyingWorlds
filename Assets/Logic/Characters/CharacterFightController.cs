using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CharacterFightController : MonoBehaviour {

	[SerializeField] Portrait portrait;
	[SerializeField] Text t_Health, t_Range, t_Damage;
	private Character character;

	public void LoadCharacterIn(Character character){
		if (character != null) {
			this.gameObject.SetActive (true);
			this.character = character;
			portrait.character = character;
			StopAllCoroutines ();
			UpdateStats ();

		} else {
			Debug.Log("Character is null");
			this.gameObject.SetActive (false);
		}
	}

	private void UpdateStats(){
		t_Damage.text = "Damage:"+character.Damage.ToString();
		t_Range.text = "Range:"+character.WeaponItem.Range.ToString();
		t_Health.text = "Health:"+character.Health.ToString()+"/"+character.MaxHealth.ToString();
	}
	private void Update(){
		if (enabled && character!=null) {
			UpdateStats();
		}
	}

	public void Flee(){
		if (character != null) {

			character.fightState = FightState.Flee;
			character.Tick();
		}
	}
	public void FallBack(){
		if (character != null && character.fightState != FightState.Flee) {
			character.fightState = FightState.Fallback;
			character.currentTarget = null;
			character.Tick();
		}
	}
	public void StandGround(){
		if (character != null && character.fightState != FightState.Flee) {
			character.fightState = FightState.StandGround;
			character.currentTarget = null;
			character.Tick();
		}
	}
	public void Attack(){
		if (character != null && character.fightState != FightState.Flee) {
			character.fightState = FightState.Attack;
			character.currentTarget = null;

			character.Tick();
		}
	}
}
