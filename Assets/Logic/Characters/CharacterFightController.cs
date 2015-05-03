using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CharacterFightController : MonoBehaviour {

	[SerializeField] Portrait portrait;

	[SerializeField] HealthBar healthBar;
	private Character character;

	public void LoadCharacterIn(Character character){
		if (character != null) {
			this.gameObject.SetActive (true);
			this.character = character;
			portrait.character = character;
			StopAllCoroutines ();
			UpdateStats ();

		} else {

			this.gameObject.SetActive (false);
		}
	}

	private void UpdateStats(){

		healthBar.UpdateStatus (character.MaxHealth, character.Health);

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
		if (character != null && character.fightState != FightState.Flee && character.Orders>=2) {
			character.fightState = FightState.Fallback;
			character.currentTarget = null;
			character.Tick();
		}
	}
	public void StandGround(){
		if (character != null && character.fightState != FightState.Flee  && character.Orders>=1) {
			character.fightState = FightState.StandGround;
			character.currentTarget = null;
			character.Tick();
		}
	}
	public void Attack(){
		if (character != null && character.fightState != FightState.Flee && character.Orders>=3) {
			character.fightState = FightState.Attack;
			character.currentTarget = null;


		}
	}
}
