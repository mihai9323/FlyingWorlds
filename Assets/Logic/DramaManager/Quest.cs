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
			if (this.outcomePair.positiveOutcome.rewardData.rewardType == RewardData.RewardType.Item) {
				this.outcomePair.positiveOutcome.rewardData.chosenItem = this.outcomePair.negativeOutcome.rewardData.chosenItem = this.minorPicture.rewardData.chosenItem;
			} else {
				this.outcomePair.positiveOutcome.rewardData.chosenName = this.outcomePair.negativeOutcome.rewardData.chosenName = this.minorPicture.rewardData.chosenName;
			}
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
				float[] f =  new float[4]{0,0,0,0};
				
				f [0] = mChances (0);
				f [1] = mChances (1);
				f [2] = mChances (2);
				f [3] = mChances (3);

				return f;
		}

		float mChances(int index){
			return (this.questLocation.enemyTypeChances[index] + this.questTime.enemyTypeChances[index] + this.enemyData.enemyTypeChances[index]*2)/4;
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
				if(this.outcomePair.positiveOutcome.rewardData.rewardType == RewardData.RewardType.Progression){
					DramaManager.nextBoss.status = BossData.Status.encountered;
				}
				DramaManager.progression += this.qd.progressForLosing;
			}
		}


	}
}
