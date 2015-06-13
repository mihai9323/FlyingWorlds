using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour {
	public Transform RoadWorldSpace;
	public bool CharactersTravellingToFightScene;
	private static bool displayedBattleTutorial = false;
	[SerializeField] GameObject battleTutorial;

	public void OnClick(){
		if (HubManager.interactable) {
			if(DramaPack.DramaManager.noQuestSelected){
				HubManager.notification.ShowConfirm("You haven't chosen who you side with for the next quest!",
				                                     DramaPack.DramaManager.questBest.qd.questGiver.Name,
				                                     DramaPack.DramaManager.questSecondBest.qd.questGiver.Name,
				                                     delegate() {
														DramaPack.DramaManager.ActivateQuest(true);
														this.OnClick();
														HubManager.notification.Hide();
				                                     },
													 delegate() {
														DramaPack.DramaManager.ActivateQuest(false);
														this.OnClick();
														HubManager.notification.Hide();
													 });



			}else
			if (!CharacterManager.partyEmpty) {
				if(displayedBattleTutorial){
					CharactersTravellingToFightScene = true;
					CharacterManager.ChangeAllActiveStateTo(FightState.MovingToBattle);
					CharacterManager.MoveAllActiveHereAndChangeState (RoadWorldSpace.position,FightState.Waiting,
					                                                  delegate() {
																		Invoke ("LoadFightSceneDelayed",.1f);
					                                                 });
					HubManager.HideAll ();
					HubManager.interactable = false;
				}else{
					HubManager.HideAll();
					displayedBattleTutorial = true;
					battleTutorial.SetActive(true);

				}
			} else
				HubManager.notification.ShowNotification("You haven't chosen any great champions for the next battle! \n Go to the barracks and prepare your man!","To Barracks!",delegate() {
					HubManager.HideAll();
					HubManager.ShowCharacters();
				});
		}
	}

	private void LoadFightSceneDelayed(){
		CharacterManager.SetAllCharactersStaticProperties ();
		CharactersTravellingToFightScene = false;
		GameData.LoadScene(GameScenes.Fight);

	}


}
