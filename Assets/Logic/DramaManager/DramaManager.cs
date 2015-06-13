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

		public DramaPack.QuestData[] quests;
		public DramaPack.PrologueData[] prologues;
		public DramaPack.ConnectorData[] conectors;

		public static Quest questBest;
		public static Quest questSecondBest;

		public static string currentQuestStory;

		private void Start(){
			Select2Quests ();

		}

		public void Select2Quests(){

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
			currentQuestStory = QuestStoryBuilder ();
			Debug.Log (currentQuestStory);
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
		}

		public string QuestStoryBuilder(){
			string storyString = "";
			Debug.Log ("building story string");
			if (progression == 0) {
				Debug.Log("first quest");
				//This is the first quest of the game
				storyString = prologues [(int)Mathf.Clamp (Random.Range (0, prologues.Length), 0, prologues.Length - 1)].DisplayData ();
				storyString += questBest.DisplayDataBefore ();
				storyString += conectors [(int)Mathf.Clamp (Random.Range (0, conectors.Length), 0, conectors.Length - 1)].DisplayData ();
				storyString += questSecondBest.DisplayDataBefore ();
			} else {
				Debug.Log("not first quest");
				if(lastQuest.questState == Quest.QuestCompletion.FinishedSuccessful){
					Debug.Log("previous quest was succesful");
					storyString += lastQuest.DisplayDataAfter(true);
					storyString += questBest.DisplayDataBefore ();
					storyString += conectors [(int)Mathf.Clamp (Random.Range (0, conectors.Length), 0, conectors.Length - 1)].DisplayData ();
					storyString += questSecondBest.DisplayDataBefore ();
				}else{
					Debug.Log("previous quest was failed");
					storyString += lastQuest.DisplayDataAfter(false);
					storyString += questBest.retryQuestData.DisplayData();
					storyString += conectors [(int)Mathf.Clamp (Random.Range (0, conectors.Length), 0, conectors.Length - 1)].DisplayData ();
					storyString += questSecondBest.DisplayDataBefore();

				}

			}
			return storyString;
		
		}
		public void FinishQuest(bool successful){

		}



	}
}
