using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class MinorPictureData: StringData {
		
		public RewardData rewardData;
		
		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}
		
		public override string DisplayData(){
			TagReplacePair[] pairs = new TagReplacePair[2]{
				new TagReplacePair ("[rew]", rewardData),
				new TagReplacePair ("[nBoss]",DramaManager.nextBoss)
				
			};
			return base.DisplayData (pairs);
		}
		
		
	}
}