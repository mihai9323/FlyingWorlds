using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace DramaPack{
	public class DramaManager : MonoBehaviour {
		public static float progression;
		public static bool lastFailed;
		public static Quest lastQuest;
		public static Quest activeQuest;
		public static bool noQuestSelected = true;
		public DramaPack.QuestData[] quests;
		public DramaPack.PrologueData[] prologues;
		public DramaPack.ConnectorData[] conectors;


		public static int nrOfQuests;


		public static Quest questBest;
		public static Quest questSecondBest;

		public static string q1,q2,q0;

		public static BossData miniBoss;
		public static BossData endBoss;

		public static BossData nextBoss{
			get{
				if(miniBoss.status == BossData.Status.completed){
					return endBoss;
				}else return miniBoss;
			}
		}

		public BossData[] miniBossList;
		public BossData[] endBossList;

		private IEnumerator Start(){
			while (!CharacterManager.isReady) {
				yield return null;
			}
			miniBoss = miniBossList[(int)(Mathf.Clamp(Random.Range ((int)0,(int)miniBossList.Length),0,miniBossList.Length- 1))];
			endBoss = endBossList  [(int)(Mathf.Clamp(Random.Range ((int)0,(int)endBossList.Length),0, endBossList.Length - 1))];
			while (miniBoss.name == endBoss.name) {
				miniBoss = miniBossList[(int)(Mathf.Clamp(Random.Range ((int)0,(int)miniBossList.Length),0,miniBossList.Length- 1))];
				endBoss = endBossList  [(int)(Mathf.Clamp(Random.Range ((int)0,(int)endBossList.Length),0, endBossList.Length - 1))];

			}

			Select2Quests ();

		}

		private QuestData[] ShuffleQuests(QuestData[] qs){
			var rand = new System.Random();
			for (int i = qs.Length - 1; i > 0; i--)
			{
				int n = rand.Next(i + 1);
				QuestData temp = qs[i];
				qs[i] = qs[n];
				qs[n] = temp;
			}
			return qs;
		}

		public void Select2Quests(){
			nrOfQuests = 0;
			Debug.Log ("Select 2 quests");
			noQuestSelected = true;
			quests = ShuffleQuests (quests);

			var questData = (from q 
							in quests
							where (q.Fitness >= 0)
							orderby q.Fitness
			                 select q).ToList ();
			Debug.Log ("<color=red>"+questData [0].Fitness + " " + questData [1].Fitness+"</color>");
			if (questData != null && questData.Count > 0) {
				Debug.Log("Selected first quest");
				if(progression>0 && (questData[0] == lastQuest.qd || lastQuest.questState != Quest.QuestCompletion.FinishedSuccessful)){
					Debug.Log("Best quest is the same");
					questBest = lastQuest;
				}else{
					Debug.Log("Best quest is not the same");
					questBest = questData[0].GenerateQuest();
				}
				nrOfQuests = 1;

				if(questData.Count>1){
					questSecondBest = null;
					foreach(QuestData qd in questData){
						if(qd.questGiver.Traits != questBest.qd.questGiver.Traits &&
						   qd.minorPictureParent != questBest.qd.minorPictureParent){
							nrOfQuests = 2;
							Debug.Log("Selected second quests");
							questSecondBest = qd.GenerateQuest();
							Debug.Log(questSecondBest.name);
							break;
						}
					}
					if(questSecondBest == null){
						foreach(QuestData qd in questData){
							if(qd.questGiver.Traits != questBest.qd.questGiver.Traits){
								nrOfQuests = 2;
								Debug.Log("Selected second quests");
								questSecondBest = qd.GenerateQuest();
								Debug.Log(questSecondBest.name);
								break;
							}
						}
					}

				}else{
					questSecondBest = null;
				}
			}else{
				questBest = null;
			}
			QuestStoryBuilder ();

			Debug.Log(questSecondBest.name);
			Debug.Log ("number of quests found:"+questData.Count);
			if (questData.Count == 1) {
				HubManager.questUI.Configure (questBest);
			} else if(questData.Count>1){
				HubManager.questUI.Configure (questBest, questSecondBest);
			}

		
		}

		public void ChooseActiveQuest(bool bestQuest){
			if (bestQuest) {
				activeQuest = questBest;
				Debug.Log(questBest+" "+activeQuest);
				questSecondBest.SetQuestInactive();
			} else {
				activeQuest = questSecondBest;
				Debug.Log(questSecondBest+" "+activeQuest);
				questBest.SetQuestInactive();
			}
			GameData.nextBattleID = activeQuest.battle.id;
			activeQuest.SetQuestActive ();
			HubManager.questUI.RefreshActive ();
			noQuestSelected = false;
		}
		public static void ActivateQuest(bool bestQuest){
			if (bestQuest) {
				activeQuest = questBest;
				questSecondBest.SetQuestInactive();
			} else {
				activeQuest = questSecondBest;
				questBest.SetQuestInactive();
			}
			GameData.nextBattleID = activeQuest.battle.id;
			activeQuest.SetQuestActive ();
			HubManager.questUI.RefreshActive ();
			noQuestSelected = false;
		}

		public void QuestStoryBuilder(){


			if (nrOfQuests == 1) {
				questSecondBest = questBest.qd.GenerateQuest ();
				Debug.Log ("Second quest was null");
			} else {
				while(questBest.questLocation == questSecondBest.questLocation){
					questSecondBest = questSecondBest.qd.GenerateQuest ();
				}
			}
			Debug.Log ("building story string");
			if (progression == 0) {
				Debug.Log("first quest");
				//This is the first quest of the game
				q0 = prologues [(int)Mathf.Clamp (Random.Range (0, prologues.Length), 0, prologues.Length - 1)].DisplayData ();
				q1 = questBest.DisplayDataBefore ();
				q2 = questSecondBest.DisplayDataBefore ();
			} else {
				Debug.Log("not first quest");
				if(lastQuest.questState == Quest.QuestCompletion.FinishedSuccessful){

					q0 = lastQuest.outcomePair.positiveOutcome.DisplayData();
					q1 = questBest.DisplayDataBefore ();
				//	q0 += conectors [(int)Mathf.Clamp (Random.Range (0, conectors.Length), 0, conectors.Length - 1)].DisplayData ();
					q2 = questSecondBest.DisplayDataBefore ();
				}else{
					Debug.Log("previous quest was failed");
					q0 = lastQuest.outcomePair.negativeOutcome.DisplayData();
					q1 = questBest.retryQuestData.DisplayData();
				//	q0 += conectors [(int)Mathf.Clamp (Random.Range (0, conectors.Length), 0, conectors.Length - 1)].DisplayData ();
					q2 = questSecondBest.DisplayDataBefore();

				}

			}

		
		}
		public void FinishQuest(bool successful){
			Debug.Log ("FINISHED QUEST");
			Debug.Log (activeQuest);
			lastQuest = activeQuest;
			activeQuest.FinishQuest (successful);
			lastFailed = !successful;
			Select2Quests ();
		}



		public static void CheckGameWin ()
		{
			if (nextBoss.status == BossData.Status.completed) {
				Application.LoadLevel (Application.loadedLevel + 1);
			}
		}
	}
}
