﻿using UnityEngine;
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
			Select2Quests ();

		}

		public void Select2Quests(){
			noQuestSelected = true;
			var questData = (from q 
							in quests
							where (q.Fitness >= 0)
							orderby q.Fitness
			                 select q).ToList ();

			if (questData != null && questData.Count > 0) {
				Debug.Log("Selected first quest");
				if(lastQuest!=null && questData[0] == lastQuest.qd){
					questBest = lastQuest;
				}else{
					questBest = questData[0].GenerateQuest();
				}


				if(questData.Count>1){
					Debug.Log("Selected second quests");
					questSecondBest = questData[1].GenerateQuest();
					Debug.Log(questSecondBest.name);
				}else{
					questSecondBest = null;
				}
			}else{
				questBest = null;
			}
			QuestStoryBuilder ();

			Debug.Log(questSecondBest.name);
			Debug.Log ("number of quests found"+questData.Count);
			if (questData.Count == 1) {
				HubManager.questUI.Configure (questBest);
			} else if(questData.Count>1){
				HubManager.questUI.Configure (questBest, questSecondBest);
			}

		
		}

		public void ChooseActiveQuest(bool bestQuest){
			if (bestQuest) {
				activeQuest = questBest;
				questSecondBest.SetQuestInactive();
			} else {
				activeQuest = questSecondBest;
				questBest.SetQuestInactive();
			}
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
			activeQuest.SetQuestActive ();
			HubManager.questUI.RefreshActive ();
			noQuestSelected = false;
		}

		public void QuestStoryBuilder(){

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

					q0 = lastQuest.DisplayDataAfter(true);
					q1 = questBest.DisplayDataBefore ();
					q0 += conectors [(int)Mathf.Clamp (Random.Range (0, conectors.Length), 0, conectors.Length - 1)].DisplayData ();
					q2 = questSecondBest.DisplayDataBefore ();
				}else{
					Debug.Log("previous quest was failed");
					q0 = lastQuest.DisplayDataAfter(false);
					q1 = questBest.retryQuestData.DisplayData();
					q0 += conectors [(int)Mathf.Clamp (Random.Range (0, conectors.Length), 0, conectors.Length - 1)].DisplayData ();
					q2 += questSecondBest.DisplayDataBefore();

				}

			}

		
		}
		public void FinishQuest(bool successful){
			activeQuest.FinishQuest (successful);
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