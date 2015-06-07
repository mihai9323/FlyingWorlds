using UnityEngine;
using System.Collections;

public class StaticDramaManager : MonoBehaviour {

	public StaticQuest[] quests;
	public int currentQuestNumber;

	public static StaticQuest currentQuest;
	private void Start(){
		currentQuestNumber = 0;
		currentQuest = quests [currentQuestNumber];
		GameData.nextBattleID = quests[currentQuestNumber].battle.id;
		HubManager.questUIStatic.Configure(quests[currentQuestNumber]);
	
	}


	public void FinishQuest(bool success){
		if (success) {
			quests[currentQuestNumber].ApplyReward();
			currentQuestNumber++;

			if(currentQuestNumber>=quests.Length){
				Application.LoadLevel(Application.loadedLevel+1);
			}else{
				currentQuest = quests [currentQuestNumber];
				GameData.nextBattleID = quests[currentQuestNumber].battle.id;
				HubManager.questUIStatic.Configure(quests[currentQuestNumber]);

			}
		}
	}
}
