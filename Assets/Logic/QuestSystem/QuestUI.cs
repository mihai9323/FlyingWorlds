using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour {

	[SerializeField] Text questText;
	[SerializeField] Portrait characterPortrait;
	[SerializeField] Image QuestImage;
	private Quest quest;

	public void Configure(Quest quest){
		this.quest = quest;

		characterPortrait.character = quest.questGiver;
		questText.text = quest.calculateQuestString ();
		QuestImage.sprite = quest.battle.fightBackground.GetComponent<SpriteRenderer> ().sprite;
		QuestImage.color = quest.battle.dayColor;
	}
	void Start(){
		//characterPortrait.gameObject.SetActive (false);
	}





}
