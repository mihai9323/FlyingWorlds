using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
		public virtual Object RandomDramaData(Object[] dataArray, List<Object> exclData = null){
			if(exclData == null){
				if(dataArray == null) return null;
				if(dataArray.Length == 0) return null;
				return dataArray[Mathf.Clamp(Random.Range(0, dataArray.Length),0, dataArray.Length-1)];
			}else{
				if(dataArray == null || dataArray.Length == 0) return null;
				Object data = dataArray[Mathf.Clamp(Random.Range(0, dataArray.Length),0, dataArray.Length-1)];
				int tries = 0;
				while(exclData.Contains(data)){
					tries++;
					data = dataArray[Mathf.Clamp(Random.Range(0, dataArray.Length),0, dataArray.Length-1)];
					if(tries >50){
						Debug.LogWarning("Random Drama Data timed out. To many exclData instances generated");
						return null;
					}
				}
				return data;
			}
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
