using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class DramaManager : MonoBehaviour {
	[SerializeField] Quest[] _gameQuests;

	public static List<Quest> gameQuests;

	[SerializeField] Boss[] _bossList;
	public static Dictionary<Boss.BossType, Boss> bossList;

	[SerializeField] Location[] _locationList;
	public static Dictionary<Location.LocationType, Location> locationList;

	[SerializeField] Moment[] _momentList;
	public static Dictionary<Moment.MomentType, Moment> momentList;

	public static bool hasItemQuest, hasFarmQuest, hasShopQuest, hasMiniBossQuest;
	public static Quest currentQuest;
	public static Quest previousQuest;
	public static int numberOfQuests;
	public static float maxAppearances{
		get{
			float max = 0;
			foreach(Quest q in gameQuests){
				if(q.appearances>max){
					max = q.appearances;
				}
			}
			return max;
		}
	}
	public static float minAppearances{
		get{
			float min = float.MaxValue;
			foreach(Quest q in gameQuests){
				if(q.appearances<min){
					min = q.appearances;
				}
			}
			return min;
		}
	}

	public static BossQuestInfo EndBossQuest,MiniBossQuest;

	private void Awake(){
		numberOfQuests = 0;
		previousQuest = null;
		EndBossQuest = null;
		MiniBossQuest = null;
		currentQuest = null;
		hasItemQuest = hasFarmQuest = hasShopQuest = hasMiniBossQuest = false;
		bossList = new Dictionary<Boss.BossType, Boss> ();
		foreach (Boss b in _bossList) {
			bossList.Add (b.bossType,b);
		}

		locationList = new Dictionary<Location.LocationType, Location> ();
		foreach (Location l in _locationList) {
			locationList.Add (l.locationType,l);
		}

		momentList = new Dictionary<Moment.MomentType, Moment> ();
		foreach (Moment m in _momentList) {
			momentList.Add (m.momentType,m);
		}

		gameQuests = new List<Quest> ();
		foreach (Quest q in _gameQuests) {
			q.appearances = Random.value;
			gameQuests.Add(q);
		}

	}
	private IEnumerator Start(){
		yield return null;
		var endQuests = (from q in gameQuests where q.questType == Quest.QuestType.EndBossQuest select q).ToList ();
		if (endQuests != null && endQuests.Count > 0) {
			
			Quest endBossQuest = endQuests[(int)Random.Range(0,endQuests.Count)];
			while(!CharacterManager.charactersByTrait.ContainsKey(endBossQuest.giverTrait)){
				endBossQuest = endQuests[(int)Random.Range(0,endQuests.Count)];
			}
			endBossQuest.GenerateBossQuest();
			EndBossQuest = new BossQuestInfo (endBossQuest);
		}
		var mBossQuest = (from q in gameQuests where q.questType == Quest.QuestType.MinibossQuest select q).ToList ();
		if (mBossQuest != null && mBossQuest.Count > 0) {
			Quest miniBossQuest = mBossQuest[(int)Random.Range(0,mBossQuest.Count)];
			while(!CharacterManager.charactersByTrait.ContainsKey(miniBossQuest.giverTrait)){
				miniBossQuest = mBossQuest[(int)Random.Range(0,mBossQuest.Count)];
			}
			miniBossQuest.GenerateBossQuest();
			MiniBossQuest = new BossQuestInfo (miniBossQuest);
			
		}


		currentQuest = SortQuestsByFitness () [0];
		currentQuest.GenerateQuest();

		Debug.Log (currentQuest.questType + " " + currentQuest.giverTrait);
		GameData.nextBattleID = currentQuest.battle.id;
		HubManager.questUI.Configure (currentQuest);
	}
	private List<Quest> SortQuestsByFitness(){
		var quests = (from q in gameQuests orderby q.Fitness select q).ToList ();
		return quests;
	}
	public void FinishQuest(bool success){
		numberOfQuests ++;
		if (success) {
			switch (currentQuest.questType) {
			case Quest.QuestType.FarmQuest: hasFarmQuest = true; break;
			case Quest.QuestType.ItemQuest: hasItemQuest = true; break;
			case Quest.QuestType.MinibossQuest: hasMiniBossQuest = true; break;
			case Quest.QuestType.ShopQuest: hasShopQuest = true; break;
			}
		}
		currentQuest.FinishQuest (success);
		previousQuest = currentQuest;
		currentQuest = SortQuestsByFitness () [0];
		currentQuest.GenerateQuest();

		GameData.nextBattleID = currentQuest.battle.id;
		Debug.Log (currentQuest.questType + " " + currentQuest.giverTrait);
		HubManager.questUI.Configure (currentQuest);	
	}
}
[System.Serializable]
public class Boss{
	public enum BossType{
		none,
		miniWizard, Wizard,
		miniDevil, Devil,
		miniKnight, Knight,
		miniPirate, Pirate
	}
	public BossType bossType;
	public string name;
	public string[] actions;
	public string GetRandomAction{
		get{
			return actions[Random.Range(0,actions.Length)];
		}
	}
	public Enemy prefab;
}
[System.Serializable] 
public class Location{
	public enum LocationType{
		Forrest,
		Desert,
		Volcano,
		Castle,
		MagicTower,
		PirateBay
	}
	public LocationType locationType;
	public GameObject locationBackground;
	public string locationName;
	public float[] monsterChances;
	public string[] itemRewardName;
	public string GetRandomRewardName(){
		if (itemRewardName.Length > 0) {
			return itemRewardName[(int)Random.Range(0,itemRewardName.Length)];
		}return "";
	}
	public string defaultAttacker;
}
[System.Serializable]
public class Moment{
	public enum MomentType{
		Morning,
		Day,
		Night
	}
	public MomentType momentType;
	public string momentName;
	public Color momentColor;
	public float[] monsterChances;
}

public class BossQuestInfo{
	public Location location;
	public Boss boss;
	public Moment moment;
	public string monsterAction;

	public BossQuestInfo(Quest quest){
		this.location = quest.location;
		this.boss = quest.boss;
		this.moment = quest.moment;
		this.monsterAction = quest.monsterAction;

	}
}
