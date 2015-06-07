using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace DramaPack{

	enum QuestType{


	}

	/// <summary>
	/// Data defined for the quests, used to generate quests for the user
	/// </summary>
	public class QuestData : StringData {



		public LocationData[] randomLocations;
		public LocationData[] questLocations;
		public MomentData[] questMoments;
		public OutcomePair[] outcomePairs;
		public MinorPictureData[] minorPictures;
		public EnemyData[] enemies;


		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}

		public DramaPack.Quest GenerateQuest(){
			LocationData rLoc = randomDramaData (randomLocations) as LocationData;
			LocationData cLoc = randomDramaData (questLocations) as LocationData;
			MomentData cMoment = randomDramaData (questMoments) as MomentData;
			OutcomePair outcomePair = randomDramaData (outcomePairs) as OutcomePair;
			MinorPictureData mpData = randomDramaData (minorPictures) as MinorPictureData;
			EnemyData eData = randomDramaData (enemies) as EnemyData;

			if (cLoc == null || cMoment == null || outcomePair == null || mpData == null || eData == null) {

				return null;
			}
			return new DramaPack.Quest(rLoc,cLoc,cMoment,eData,outcomePair,mpData,this.name,this.detailString);
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
	}

}