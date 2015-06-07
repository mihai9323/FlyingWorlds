using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace DramaPack{
	public class StrategyData : StringData {

		public TraitManager.TraitTypes[] applyToTraits;


		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}
		

		public override string DisplayData(TagReplacePair[] pairs)
		{
			string str = detailString;
			string championName = championNameWithTrait ();
			str = str.Replace ("[trait]",championName);
			foreach (TagReplacePair trp in pairs) {
				trp.stringData.detailString = trp.stringData.DisplayData();
				str = str.Replace(trp.tag, trp.stringData.detailString);
			}
			return str;
		}
		public override string DisplayData(){
			List<TagReplacePair> pairs = new List<TagReplacePair> ();


			
			return DisplayData (pairs.ToArray());
		}

		private string championNameWithTrait(){
			foreach (TraitManager.TraitTypes t in applyToTraits) {
				if(CharacterManager.charactersByTrait.ContainsKey(t)){
					if(CharacterManager.charactersByTrait[t] != null && CharacterManager.charactersByTrait[t].Count >0){
						return CharacterManager.charactersByTrait[t][0].Name;
					}
				}
			}
			return TraitManager.GetTrait(applyToTraits[0]).name;
		}
	
	}
}