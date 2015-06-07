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
			TagReplacePair[] pairs = new TagReplacePair[1]{
				new TagReplacePair ("[rew]", rewardData),
				
			};
			return base.DisplayData (pairs);
		}
		
		
	}
}