using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DramaPack;

public class QuestUI : MonoBehaviour {

	[SerializeField] Text questText1;
	[SerializeField] Text questText2;
	[SerializeField] Text questText0;
	[SerializeField] Portrait characterPortrait1;
	[SerializeField] Portrait characterPortrait2;
	[SerializeField] Image Quest1Image;
	[SerializeField] Image Quest2Image;

	[SerializeField] Image button1,button2;
	private Quest quest1,quest2;

	private bool questTextWritten;
	private int lastChar;
	private bool configured;

	public void Configure(Quest quest1,Quest quest2){
		Debug.Log ("Configure 2 quests");
		configured = true;
		this.quest1 = quest1;
		this.quest2 = quest2;

		characterPortrait1.character = quest1.qd.questGiver;
		characterPortrait2.character = quest2.qd.questGiver;

		Quest1Image.sprite = quest1.battle.fightBackground.GetComponent<SpriteRenderer> ().sprite;
		Quest1Image.color = quest1.battle.dayColor;
		Quest2Image.sprite = quest2.battle.fightBackground.GetComponent<SpriteRenderer> ().sprite;
		Quest2Image.color = quest2.battle.dayColor;

		questTextWritten = false;
		lastChar = 0;

		questText0.text = "";
		questText1.text = "";
		questText2.text = "";

		if (this.gameObject.activeInHierarchy) {
			StopAllCoroutines ();
			StartCoroutine (WriteText ());
		}
		RefreshActive ();
		characterPortrait1.LoadCharacter ();
		characterPortrait2.LoadCharacter ();
	}
	public void Configure(Quest quest1){
		Debug.Log ("Configure 1 quest");
		configured = true;
		this.quest1 = quest1;

		
		characterPortrait1.character = quest1.qd.questGiver;
		characterPortrait2.character = null;
		
		Quest1Image.sprite = quest1.battle.fightBackground.GetComponent<SpriteRenderer> ().sprite;
		Quest1Image.color = quest1.battle.dayColor;
		Quest2Image.color = new Color(0,0,0,0);
		
		questTextWritten = false;

		lastChar = 0;
		
		questText0.text = "";
		questText1.text = "";
		questText2.text = "";
		if (this.gameObject.activeInHierarchy) {
			StopAllCoroutines ();
			StartCoroutine (WriteText ());
		}
		RefreshActive ();
		characterPortrait1.LoadCharacter ();
	}
	public void RefreshActive(){
		if (DramaManager.questBest.questState == Quest.QuestCompletion.Active) {
			button1.color = Color.green;
			button2.color = Color.white;

		} else if (DramaManager.questSecondBest.questState == Quest.QuestCompletion.Active) {
			button1.color = Color.white;
			button2.color = Color.green;
		} else {
			button1.color = button2.color = Color.white;
		}
	}



	private void OnEnable(){
		if (configured) {
			if (questTextWritten) {
				this.questText0.text = DramaManager.q0;
				this.questText1.text = DramaManager.q1;
				this.questText2.text = DramaManager.q2;
			} else {
				StopAllCoroutines ();
				StartCoroutine (WriteText ());
			}
		}
	}
	private IEnumerator WriteText(){
		while (!HubManager.interactable) {
			yield return null;
		}
		HubManager.interactable = false;
		this.questText0.text = this.questText1.text = this.questText2.text = "";
		for(lastChar = 0; lastChar<DramaManager.q0.Length; lastChar++){
			this.questText0.text+= DramaManager.q0[lastChar];
			float t = Random.value;
			if(t<.15f){
				yield return new WaitForSeconds(t); 
			}
		}
		for(lastChar = 0; lastChar<DramaManager.q1.Length; lastChar++){
			this.questText1.text+= DramaManager.q1[lastChar];
			float t = Random.value;
			if(t<.15f){
				yield return new WaitForSeconds(t); 
			}
		}
		for(lastChar = 0; lastChar<DramaManager.q2.Length; lastChar++){
			this.questText2.text+= DramaManager.q2[lastChar];
			float t = Random.value;
			if(t<.15f){
				yield return new WaitForSeconds(t); 
			}
		}
		questTextWritten = true;
		HubManager.interactable = true;
	}






}
