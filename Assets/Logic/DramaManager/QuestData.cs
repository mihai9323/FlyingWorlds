using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
namespace DramaPack{


	/// <summary>
	/// Data defined for the quests, used to generate quests for the user
	/// </summary>
	public class QuestData : StringData {

		private int timesPlayed;

		public bool isBossQuest;

		[TextArea(5,10)]public string revengeString;

		public float minProgressForQuest;
		public float progressForWinning;
		public float progressForLosing;




		public LocationData[] randomLocations;
		public LocationData[] questLocations;
		public MomentData[] questMoments;
		public OutcomePair[] outcomePairs;


		public GameObject minorPictureParent;
		public MinorPictureData[] minorPictures;
		public EnemyData[] enemies;
		public RetryQuestData[] retryQuestsData;
		public float[] traitRelevance;
		public TraitManager.TraitTypes[] relevantTraits;

		private Dictionary<TraitManager.TraitTypes,float> relevanceTraitPair;




	
		private List<Character> relevantCharacters{
			get{
				if(_relevantCharacters!=null && _relevantCharacters.Count>0) return _relevantCharacters;
				else{
					_relevantCharacters = new List<Character> ();
					foreach (TraitManager.TraitTypes t in relevantTraits) {
						if(CharacterManager.charactersByTrait.ContainsKey(t)){
							foreach(Character c in CharacterManager.charactersByTrait[t]){
								_relevantCharacters.Add(c);
							}
						}
					}

					return _relevantCharacters;
				}

			}
		}
		private List<Character> _relevantCharacters;
		public Character questGiver;

		private void Awake(){
			if (traitRelevance.Length < relevantTraits.Length) {
				traitRelevance = new float[relevantTraits.Length];

			}
			relevanceTraitPair = new Dictionary<TraitManager.TraitTypes, float> ();
			for (int i = 0; i<relevantTraits.Length; i++) {
				if(!relevanceTraitPair.ContainsKey(relevantTraits[i])){
				relevanceTraitPair.Add(relevantTraits[i],traitRelevance[i]);
				}
			}
		}
		IEnumerator Start(){
			timesPlayed = 0;
			while(!CharacterManager.isReady){
				yield return null;
			}


			_relevantCharacters = new List<Character> ();
			foreach (TraitManager.TraitTypes t in relevantTraits) {
				if(CharacterManager.charactersByTrait.ContainsKey(t)){
					foreach(Character c in CharacterManager.charactersByTrait[t]){
						_relevantCharacters.Add(c);
					}
				}
			}
		}
		// Use this for initialization
		protected override void Update () {
			base.Update ();
			if(minorPictureParent!=null)minorPictures = minorPictureParent.gameObject.GetComponentsInChildren<MinorPictureData> ();
		}

		public DramaPack.Quest GenerateQuest(){
			this.timesPlayed++;
			LocationData rLoc = randomDramaData (randomLocations) as LocationData;
			LocationData cLoc = randomDramaData (questLocations) as LocationData;
			while (rLoc.name == cLoc.name) {
				rLoc = randomDramaData (randomLocations) as LocationData;
				cLoc = randomDramaData (questLocations) as LocationData;
			}
			MomentData cMoment = randomDramaData (questMoments) as MomentData;
			OutcomePair outcomePair = randomDramaData (outcomePairs) as OutcomePair;
			MinorPictureData mpData = randomDramaData (minorPictures) as MinorPictureData;
			EnemyData eData = randomDramaData (enemies) as EnemyData;
			RetryQuestData retryQuestData = randomDramaData (retryQuestsData) as RetryQuestData;

			if (cLoc == null || cMoment == null || outcomePair == null || mpData == null || eData == null) {
				Debug.Log("something is null");
				return null;
			}
			string detail = this.detailString;
			if (mpData.rewardData.rewardType == RewardData.RewardType.Progression && DramaManager.nextBoss.status == BossData.Status.encountered) {
				detail = revengeString;
			}
			GameObject g = new GameObject ("quest");
			Quest q = g.AddComponent<Quest> ();
			q.ConfigQuest(rLoc,cLoc,cMoment,eData,outcomePair,mpData,this.name,detail,this,retryQuestData);
			return q;


		}

		public Object randomDramaData(Object[] dataArray, List<Object> exclData = null){
			if(exclData == null){
				if(dataArray == null) return null;
				if(dataArray.Length == 0) return null;
				return dataArray[Mathf.Clamp(Random.Range(0, dataArray.Length),0, dataArray.Length-1)];
			}else{
				if(dataArray == null || dataArray.Length == 0) return null;
				Object data = dataArray[Mathf.Clamp(Random.Range(0, dataArray.Length),0, dataArray.Length-1)];
				int tries = 0;
				while(exclData.Contains(data)){
					tries++;
					data = dataArray[Mathf.Clamp(Random.Range(0, dataArray.Length),0, dataArray.Length-1)];
					if(tries >50){
						Debug.LogWarning("Random Drama Data timed out. To many exclData instances generated");
						return null;
						}
				}
				return data;
			}
		}

		public float Fitness{
			get{
				bool isBestFit = false;
				bool isNoFit= false;
				float questDiversity = 0;
				if(DramaManager.progression>0){
					if(DramaManager.lastFailed && DramaManager.lastQuest.qd == this){
						Debug.Log ("Receive the same quest as last time");
						isBestFit = true; //best fit => force the ability to restart the quest
					}
					if(!DramaManager.lastFailed && DramaManager.lastQuest.qd == this){
						Debug.Log("received quest last time");
						isNoFit = true; //avoid getting the same quest 2 times in a row
					}
					if(this.minorPictures[0].rewardData.rewardType == DramaManager.lastQuest.minorPicture.rewardData.rewardType){
						questDiversity = 2;
						Debug.Log("<color=blue>The quests are not diverse enough</color>");
					}
				}else{
					Debug.Log("No last quest");
				}
				float progressScore = DramaManager.progression - minProgressForQuest; 
				if(progressScore <0) isNoFit = true; //we did not progress enough to get this quest
				float timesEncounteredScore = (float)(timesPlayed);
				float traitScore = 0;
				if(relevantCharacters != null && relevantCharacters.Count>0){

					Character bestCharacter = relevantCharacters[0];
					float bestScore = 0;
					for(int i =0;i<relevantCharacters.Count; i++){
						float f1= 0.01f,f2 = 0.01f;
						if(relevanceTraitPair.ContainsKey(relevantCharacters[i].Traits[0])){
							f1 =  relevanceTraitPair[relevantCharacters[i].Traits[0]];
						}
						if(relevanceTraitPair.ContainsKey(relevantCharacters[i].Traits[1])){
							f2 =  relevanceTraitPair[relevantCharacters[i].Traits[1]];
						}
						float relevance = Mathf.Max(f1,f2);
						float r = relevantCharacters[i].CalculateMoral();// * relevance;
						if(bestScore < r){
							bestScore = r;
							bestCharacter = relevantCharacters[i];
						}
					}
					Debug.Log(bestCharacter);
					this.questGiver = bestCharacter;
					traitScore = 1-bestScore;
				}else{
					Debug.Log("no quest giver");
					isNoFit = true; //there are no quest givers => we can not get this quest
				}
				if(isNoFit){
					Debug.Log("Fittness ["+this.name+"]["+(-1).ToString()+"]");
					return -1;
				}
				if(isBestFit){
					Debug.Log("Fittness ["+this.name+"]["+(0).ToString()+"]");
					return 0;
				}

				Debug.Log("Fittness ["+this.name+"]["+(progressScore + timesEncounteredScore + traitScore).ToString()+"]");
				return progressScore + timesEncounteredScore + traitScore + questDiversity;
			}
		}

	}

}