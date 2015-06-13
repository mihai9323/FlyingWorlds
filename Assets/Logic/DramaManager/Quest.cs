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
		public QuestData qd;
		public RetryQuestData retryQuestData;

		public Battle battle;
		public enum QuestCompletion
		{
			NotSelected,
			Active,
			FinishedSuccessful,
			FinishedFailed
		}
		public QuestCompletion questState;

		public Quest(LocationData randomLocation, LocationData questLocation, MomentData questTime, EnemyData enemyData, OutcomePair outcomePair, MinorPictureData minorPicture,string name, string detailString, QuestData qd, RetryQuestData retryQuestData){

			this.randomLocation = randomLocation;
			this.questLocation = questLocation;
			this.questTime = questTime;
			this.enemyData = enemyData;
			this.outcomePair = outcomePair;
			this.minorPicture = minorPicture;
			this.name = name;
			this.detailString = detailString;
			this.qd = qd;
			this.questState = QuestCompletion.NotSelected;
			this.retryQuestData = retryQuestData;
			this.minorPicture.rewardData.GenerateName ();
			this.outcomePair.positiveOutcome.rewardData.chosenItem = this.minorPicture.rewardData.chosenItem;
			this.outcomePair.negativeOutcome.rewardData.chosenName = this.minorPicture.rewardData.chosenName;
			this.battle = new Battle (Time.time.ToString(), this.questLocation.name, this.questTime.name, this.questLocation.background, this.questTime.timeColor, this.MonsterChances ());
			if (FightManager.battles.ContainsKey (battle.id.ToString())) {
				FightManager.battles[battle.id.ToString()] = this.battle;
			} else {
				FightManager.battles.Add (battle.id.ToString (), battle);
			}

		}
		public void SetQuestActive(){
			this.questState = QuestCompletion.Active;
			GameData.nextBattleID = this.battle.id;
		}
		public void SetQuestInactive(){
			this.questState = QuestCompletion.NotSelected;
		}
		public float[] MonsterChances(){
				return new float[4]{.25f,.25f,.25f,.25f};
		}
		/// <summary>
		///  Displays data relevant to the quest before it is started
		/// </summary>
		/// <returns>The data before.</returns>
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

		/// <summary>
		/// Displays data relevant to the quest after it was completed
		/// </summary>
		/// <returns>The data after.</returns>
		/// <param name="success">If set to <c>true</c> success.</param>
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

		public void FinishQuest(bool success){
			if (success) {
				this.questState = QuestCompletion.FinishedSuccessful;
				DramaManager.progression += this.qd.progressForWinning;
				this.outcomePair.positiveOutcome.rewardData.ApplyReward();
			} else {
				this.questState = QuestCompletion.FinishedFailed;
				DramaManager.progression += this.qd.progressForLosing;
			}
		}


	}
}
