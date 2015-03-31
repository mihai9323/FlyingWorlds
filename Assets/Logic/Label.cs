using UnityEngine;
using System.Collections;
[System.Serializable]
public class Label  {

	
	public LabelManager.LabelType labelType;
	
	
	
	[SerializeField] string dontLikeOtherString;
	[SerializeField] string dontLikeSelfString;
	[SerializeField] string likeOtherString;
	[SerializeField] string likeSelfString;
	
	public string LikeSelfString(){
		return likeSelfString;
	}
	public string LikeOtherString(string otherName){
		return likeOtherString.Replace("[name]",otherName);
	}
	
	public string DontLikeSelfString(){
		return dontLikeSelfString;
	}
	public string DontLikeOtherString(string otherName){
		return dontLikeOtherString.Replace("[name]",otherName);
	}
	
}
