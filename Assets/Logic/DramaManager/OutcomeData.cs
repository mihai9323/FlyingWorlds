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
		

			TagReplacePair[] pairs = new TagReplacePair[6]{
				new TagReplacePair ("[rew]", rewardData),
				new TagReplacePair ("[enemy]",DramaPack.DramaManager.lastQuest.enemyData),
				new TagReplacePair ("[location]",DramaPack.DramaManager.lastQuest.questLocation),
				new TagReplacePair ("[nBoss]",DramaManager.nextBoss),
				new TagReplacePair ("[mBoss]",DramaManager.miniBoss),
				new TagReplacePair("[nrew]", rewardData.chosenName)

			};
			return base.DisplayData (pairs);
		}
		
		
	}

}