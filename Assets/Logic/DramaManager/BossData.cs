using UnityEngine;
using System.Collections;
namespace DramaPack{
	public class BossData: StringData {
		public enum Status{
			notEncountered,
			encountered,
			completed

		}
		public Status status = Status.notEncountered;
		public GameObject bossObject;
		// Use this for initialization

		protected override void Update () {
			base.Update ();
		}


	}
}