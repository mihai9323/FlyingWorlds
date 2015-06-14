using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace DramaPack{
	public class RetryQuestData : StringData {

		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}

		public override string DisplayData(){
			List<TagReplacePair> pairs = new List<TagReplacePair> ();
			

			pairs.Add (new TagReplacePair ("[pmPic]", DramaPack.DramaManager.lastQuest.minorPicture));
			pairs.Add (new TagReplacePair ("[plocation]", DramaPack.DramaManager.lastQuest.questLocation));


			
			return base.DisplayData (pairs.ToArray());
		}
	}
}