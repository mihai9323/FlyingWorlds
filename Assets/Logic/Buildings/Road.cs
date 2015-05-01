using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour {
	public Transform RoadWorldSpace;

	public void OnClick(){
		if (HubManager.interactable) {
			if (!CharacterManager.partyEmpty) {
				CharacterManager.ChangeAllActiveStateTo(FightState.MovingToBattle);
				CharacterManager.MoveAllActiveHereAndChangeState (RoadWorldSpace.position,FightState.Waiting,
				                                                  delegate() {
																	
																	Invoke ("LoadFightSceneDelayed",1.2f);
				                                                 });
				HubManager.HideAll ();
				HubManager.interactable = false;

			} else
				HubManager.notification.ShowNotification("You haven't chosen any great champions for the next battle! \n Go to the barracks and prepare your man!","Let's go!",delegate() {
					HubManager.ShowCharacters();
				});
		}
	}

	private void LoadFightSceneDelayed(){
		GameData.LoadScene(GameScenes.Fight);
	}

}
