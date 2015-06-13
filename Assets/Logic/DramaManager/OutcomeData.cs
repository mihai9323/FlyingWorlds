using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class OutcomeData: StringData {

		[HideInInspector]public RewardData rewardData;

		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}

		public override string DisplayData(){
			this.detailString.Replace ("[nrew]", DramaManager.lastQuest.minorPicture.rewardData.chosenName);
			TagReplacePair[] pairs = new TagReplacePair[5]{
				new TagReplacePair ("[rew]", rewardData),
				new TagReplacePair ("[enemy]",DramaPack.DramaManager.lastQuest.enemyData),
				new TagReplacePair ("[location]",DramaPack.DramaManager.lastQuest.questLocation),
				new TagReplacePair ("[nBoss]",DramaManager.nextBoss),
				new TagReplacePair ("[mBoss]",DramaManager.miniBoss)


			};
			return base.DisplayData (pairs);
		}
		
		
	}

}