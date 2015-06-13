using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class MomentData : StringData {

		public float[] enemyTypeChances = new float[4]{0.25f,0.25f,0.25f,0.25f};
		public Color timeColor;
		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}
		
		
	}
}