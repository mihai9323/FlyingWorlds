using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour {

	[SerializeField] Text questText;
	[SerializeField] Portrait characterPortrait;
	[SerializeField] Image QuestImage;
	private Quest quest;

	private bool questTextWritten;
	private int lastChar;
	private bool configured;
	public void Configure(Quest quest){
		configured = true;
		this.quest = quest;
		questTextWritten = false;
		lastChar = 0;
		characterPortrait.character = quest.questGiver;
		questText.text = "";
		QuestImage.sprite = quest.battle.fightBackground.GetComponent<SpriteRenderer> ().sprite;
		QuestImage.color = quest.battle.dayColor;
		if (this.gameObject.activeInHierarchy) {
			StopAllCoroutines ();
			StartCoroutine (WriteText (this.quest.calculateQuestString ()));
		}
	}
	private void OnEnable(){
		if (configured) {
			if (questTextWritten) {
				this.questText.text = quest.calculateQuestString ();
			} else {
				StopAllCoroutines ();
				StartCoroutine (WriteText (this.quest.calculateQuestString ()));
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
			if(t<.2f){
				yield return new WaitForSeconds(t); 
			}
		}
		questTextWritten = true;
		HubManager.interactable = true;
	}





}
