using UnityEngine;
using System.Collections;
namespace DramaPack{
	[System.Serializable]
	public class StringData : DramaData {

		[TextArea(5,10)] public string detailString;
		

		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}

		public virtual string DisplayData(TagReplacePair[] pairs)
		{
			string str = detailString;
			Debug.Log (str);
			str = str.Replace ("[name]", this.name);
			foreach (TagReplacePair trp in pairs) {
				trp.stringData.detailString = trp.stringData.DisplayData();
				str = str.Replace(trp.tag, trp.stringData.detailString);
			}
			return str;
		}
		public virtual string DisplayData(){
			return detailString;
		}


	}

	[System.Serializable]
	public class TagReplacePair{
		public string tag;
		public StringData stringData;

		public TagReplacePair(string tag, StringData stringData){
			this.tag = tag;
			this.stringData = stringData;
		}
	}
}
