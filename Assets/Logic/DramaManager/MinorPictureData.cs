using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class MinorPictureData: StringData {

		public GameObject possibleRewardsParent;
		public RewardData[] possibleRewards;

		[HideInInspector]public RewardData rewardData;
		
		// Use this for initialization
		protected override void Update () {
			base.Update ();
			if(possibleRewardsParent!=null)possibleRewards = possibleRewardsParent.gameObject.GetComponentsInChildren<RewardData> ();
		}
		
		public override string DisplayData(){
			TagReplacePair[] pairs = new TagReplacePair[2]{
				new TagReplacePair ("[rew]", rewardData),
				new TagReplacePair ("[nBoss]",DramaManager.nextBoss)
				
			};
			return base.DisplayData (pairs);
		}

		public void ChooseRewardFromArray(){
			rewardData = possibleRewards[(int)Mathf.Clamp(Random.Range(0,possibleRewards.Length),0,possibleRewards.Length-1)];
		}
		
		
	}
}