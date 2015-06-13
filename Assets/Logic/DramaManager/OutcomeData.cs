using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class OutcomeData: StringData {

		public RewardData rewardData;

		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}

		public override string DisplayData(){
			TagReplacePair[] pairs = new TagReplacePair[3]{
				new TagReplacePair ("[rew]", rewardData),
				new TagReplacePair ("[enemy]",DramaPack.DramaManager.lastQuest.enemyData),
				new TagReplacePair ("[location]",DramaPack.DramaManager.lastQuest.questLocation),

			};
			return base.DisplayData (pairs);
		}
		
		
	}

}