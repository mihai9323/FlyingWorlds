using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class PrologueData : StringData {
		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}
		public override string DisplayData(){
			TagReplacePair[] pairs = new TagReplacePair[1]{

				new TagReplacePair ("[nBoss]",DramaManager.nextBoss)
				
			};
			return base.DisplayData (pairs);
		}
	}
}