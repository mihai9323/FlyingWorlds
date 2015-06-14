using UnityEngine;
using System.Collections;
#if ADD_DATA
using UnityEditor;
#endif

namespace DramaPack{
#if ADD_DATA 
	[ExecuteInEditMode] 
#endif
	public class DramaData : MonoBehaviour {

		public string name;
	
		#if ADD_DATA
		protected virtual void Update () {
			if (string.IsNullOrEmpty (name)) {
				name = this.GetType().ToString();
			}
			this.gameObject.name = this.name;
		}
		#else
		protected virtual void Update(){
			
		}
		#endif
	}
}