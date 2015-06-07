using UnityEngine;
using System.Collections;

public class MultipleGroup : MonoBehaviour {


	public MultipleButton[] buttons;



	public string[] getValues(){
		if (buttons == null)
			return null;
		System.Collections.Generic.List<string> valueList = new System.Collections.Generic.List<string> ();
		foreach (MultipleButton mb in buttons) {
			if(mb.isSelected){
				valueList.Add(mb.value);
			}
		}
		return valueList.ToArray ();

	}
	public int GetSelectedCount(){
		int c = 0;
		foreach (MultipleButton mb in buttons) {
			if(mb.isSelected){
				c++;
			}
		}
		return c;
	}
}
