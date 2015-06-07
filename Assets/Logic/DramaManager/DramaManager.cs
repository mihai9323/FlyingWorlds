using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class DramaManager : MonoBehaviour {
		public static int progression;

		public DramaPack.QuestData[] quests;

		private void Start(){
			var quest = quests [0].GenerateQuest ();
			Debug.Log (quest.name);
			Debug.Log (quest.detailString);
			Debug.Log (quest.DisplayDataBefore ());
		}
	}
}
