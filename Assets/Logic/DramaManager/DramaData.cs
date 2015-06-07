using UnityEngine;
using System.Collections;
using UnityEditor;


namespace DramaPack{
	[ExecuteInEditMode]
	public class DramaData : MonoBehaviour {

		public string name;
	
		#if UNITY_EDITOR
		protected virtual void Update () {
			if (string.IsNullOrEmpty (name)) {
				name = this.GetType().ToString();
			}
			this.gameObject.name = this.name;
		}
		#elif
		protected override void Update(){
			
		}
		#endif
	}
}