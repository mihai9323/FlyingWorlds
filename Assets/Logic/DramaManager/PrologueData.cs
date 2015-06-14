using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class PrologueData : StringData {
		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}
		public override string DisplayData(){
			TagReplacePair[] pairs = new TagReplacePair[3]{

				new TagReplacePair ("[nBoss]",DramaManager.nextBoss),
				new TagReplacePair ("[pmBoss]",PersistentData.previousMiniBossName),
				new TagReplacePair ("[pBoss]",PersistentData.previousEndBossName)
				
			};
			return base.DisplayData (pairs);
		}
	}
}