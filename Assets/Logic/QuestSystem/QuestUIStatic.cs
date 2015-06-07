using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestUIStatic : MonoBehaviour {

	[SerializeField] Text questText;
	[SerializeField] Portrait characterPortrait;
	[SerializeField] Image QuestImage;
	private StaticQuest quest;

	private bool questTextWritten;
	private int lastChar;
	private bool configured;
	public void Configure(StaticQuest quest){
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
			StartCoroutine (WriteText (this.quest.questString));
		}
	}



	private void OnEnable(){
		if (configured) {
			if (questTextWritten) {
				this.questText.text = this.quest.questString;
			} else {
				StopAllCoroutines ();
				StartCoroutine (WriteText (this.quest.questString));
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
	private void Update(){
		if (quest!=null && enabled && HubManager.interactable) {
			if(Input.GetMouseButtonDown (0)){
				questTextWritten = true;
				this.questText.text = this.quest.questString;
				StopAllCoroutines();
			}
		}
	}





}
