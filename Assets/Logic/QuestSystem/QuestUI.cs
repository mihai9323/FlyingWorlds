using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DramaPack;

public class QuestUI : MonoBehaviour {

	[SerializeField] Text questText;
	[SerializeField] Portrait characterPortrait1;
	[SerializeField] Portrait characterPortrait2;
	[SerializeField] Image Quest1Image;
	[SerializeField] Image Quest2Image;
	[SerializeField] Text quest1Button, quest2Button;

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

		questText.text = "";

		if (this.gameObject.activeInHierarchy) {
			StopAllCoroutines ();
			StartCoroutine (WriteText (DramaManager.currentQuestStory));
		}
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
		
		questText.text = "";
		
		if (this.gameObject.activeInHierarchy) {
			StopAllCoroutines ();
			StartCoroutine (WriteText (DramaManager.currentQuestStory));
		}
	}



	private void OnEnable(){
		if (configured) {
			if (questTextWritten) {
				this.questText.text = DramaManager.currentQuestStory;
			} else {
				StopAllCoroutines ();
				StartCoroutine (WriteText (DramaManager.currentQuestStory));
			}
		}
	}
	private IEnumerator WriteText(string writeText){
		while (!HubManager.interactable) {
			yield return null;
		}
		HubManager.interactable = false;

		for(lastChar = 0; lastChar<writeText.Length; lastChar++){
			this.questText.text+= writeText[lastChar];
			float t = Random.value;
			if(t<.15f){
				yield return new WaitForSeconds(t); 
			}
		}
		questTextWritten = true;
		HubManager.interactable = true;
	}






}
