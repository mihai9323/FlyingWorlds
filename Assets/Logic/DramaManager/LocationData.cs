using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class LocationData : StringData {

		public float[] enemyTypeChances = new float[4]{0,0,0,0};
		public GameObject background;
		// Use this for initialization
		protected override void Update () {
			base.Update ();
		}
		
		
	}
}