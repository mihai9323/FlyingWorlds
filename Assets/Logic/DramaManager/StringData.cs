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
				if(trp.stringData!=null){
					trp.stringData.detailString = trp.stringData.DisplayData();
					str = str.Replace(trp.tag, trp.stringData.detailString);
				}else if(!string.IsNullOrEmpty(trp.replaceWith)){
					str = str.Replace(trp.tag,trp.replaceWith);
					}

				}

			return str;
		}
		public virtual string DisplayData(){
			return detailString;
		}

		protected StringData GenerateStringData(){

			return GenerateStringData ("stringData");
		}
		protected StringData GenerateStringData(string gameObjectName){
			GameObject g = new GameObject ();
			g.transform.parent = this.transform;
			g.name = gameObjectName;
			return g.AddComponent<StringData> ();
		}



	}

	[System.Serializable]
	public class TagReplacePair{
		public string tag;
		public StringData stringData;
		public string replaceWith;
		public TagReplacePair(string tag, StringData stringData, string replaceWith =""){
			this.tag = tag;
			this.stringData = stringData;
			this.replaceWith = replaceWith;
		}
		public TagReplacePair(string tag,string replaceWith){
			this.tag = tag;
			this.stringData = null;
			this.replaceWith = replaceWith;
		}
	}
}
