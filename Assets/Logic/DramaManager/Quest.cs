using UnityEngine;
using System.Collections;
namespace DramaPack{
	/// <summary>
	/// A generated quest used in the game
	/// </summary>
public class Quest : StringData {

		public LocationData randomLocation;
		public LocationData questLocation;
		public MomentData questTime;
		public EnemyData enemyData;
		public OutcomePair outcomePair;
		public MinorPictureData minorPicture;

		public Quest(LocationData randomLocation, LocationData questLocation, MomentData questTime, EnemyData enemyData, OutcomePair outcomePair, MinorPictureData minorPicture,string name, string detailString){

			this.randomLocation = randomLocation;
			this.questLocation = questLocation;
			this.questTime = questTime;
			this.enemyData = enemyData;
			this.outcomePair = outcomePair;
			this.minorPicture = minorPicture;
			this.name = name;
			this.detailString = detailString;
		}
		public string DisplayDataBefore ()
		{
			if (randomLocation != null) {
				TagReplacePair[] pairs = new TagReplacePair[5]{
					new TagReplacePair ("[rLoc]", randomLocation),
					new TagReplacePair ("[cLoc]", questLocation),
					new TagReplacePair ("[cMom]", questTime),
					new TagReplacePair ("[cEne]", enemyData),
					new TagReplacePair ("[mPic]", minorPicture)
				};
				return base.DisplayData (pairs);
			} else {
				TagReplacePair[] pairs = new TagReplacePair[4]{
					new TagReplacePair ("[cLoc]", questLocation),
					new TagReplacePair ("[cMom]", questTime),
					new TagReplacePair ("[cEne]", enemyData),
					new TagReplacePair ("[mPic]", minorPicture)
				};
				return base.DisplayData (pairs);
			}
		}
		public string DisplayDataAfter(bool success){
			OutcomeData outcomeData;
			if (success)
				outcomeData = outcomePair.positiveOutcome;
			else
				outcomeData = outcomePair.negativeOutcome;
			if (randomLocation != null) {
				TagReplacePair[] pairs = new TagReplacePair[5]{
					new TagReplacePair ("[rLoc]", randomLocation),
					new TagReplacePair ("[cLoc]", questLocation),
					new TagReplacePair ("[cMom]", questTime),
					new TagReplacePair ("[cEne]", enemyData),
					new TagReplacePair ("[cOut]", outcomeData)
				};
				return base.DisplayData (pairs);
			} else {
				TagReplacePair[] pairs = new TagReplacePair[4]{
					new TagReplacePair ("[cLoc]", questLocation),
					new TagReplacePair ("[cMom]", questTime),
					new TagReplacePair ("[cEne]", enemyData),
					new TagReplacePair ("[cOut]", outcomeData)
				};
				return base.DisplayData (pairs);
			}
		}


	}
}
