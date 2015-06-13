using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace DramaPack{
	public class ConnectorData : StringData {

		[HideInInspector][SerializeField]private StringData characterName0;
		[HideInInspector][SerializeField]private StringData characterName1;

		private void Start(){
			if (characterName0 == null) {
				characterName0 = GenerateStringData("characterName0");
			}
			if (characterName1 == null) {
				characterName1 = GenerateStringData("characterName1");
			}
		}

		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}

		public override string DisplayData(){

			SetStringDataValuesToCharacterName (DramaManager.questBest.qd.questGiver, ref characterName0);
			SetStringDataValuesToCharacterName (DramaManager.questSecondBest.qd.questGiver,ref characterName1);

			List<TagReplacePair> pairs = new List<TagReplacePair> ();
			pairs.Add (new TagReplacePair("[qg1]",characterName0));
			pairs.Add (new TagReplacePair("[qg2]",characterName1));
						
			return base.DisplayData (pairs.ToArray());
		}

		private void SetStringDataValuesToCharacterName(Character c, ref StringData sd){
			sd.name = c.Name;
			sd.detailString = c.Name;
		}
	}
}