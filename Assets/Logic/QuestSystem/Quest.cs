using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Quest {

	public enum QuestType
	{
		ItemQuest,
		FarmQuest,
		ShopQuest,
		GoldQuest,
		MinibossQuest,
		EndBossQuest,

	}
	public float appearances;
	public QuestType questType;
	public Character questGiver;
	public List<Boss.BossType> possibleBossTypes;
	public List<Location.LocationType> possibleLocations;
	public List<Moment.MomentType> possibleMoments;
	public TraitManager.TraitTypes giverTrait;

	public enum QuestState
	{
		NotStarted,
		Completed,
		Failed,
		WaitingRevange
	}
	[HideInInspector]public QuestState questState;

	public string saved;
	public string attacker{
		get{
			if(string.IsNullOrEmpty(_attacker)){
				return this.location.defaultAttacker;
			}else return _attacker;
		}
	}
	[SerializeField] string _attacker;
	[SerializeField] string _rewardName;
	public string rewardName{
		get{
			if(this.questType == QuestType.ItemQuest){
				return  this.rewardItem.colorName+" "+this.rewardItem.ItemName;
			}else return _rewardName;
		}
	}
	public string firstQuest;
	public string previousQuestSuccesfulNoMiniBoss;
	public string previousQuestFailedNoMiniBoss;
	public string previousQuestSuccesfulMiniBossKilled	;
	public string previousQuestFailedMiniBossKilled;

	public string revangeQuestWithItem;
	public string revangeQuestWithoutItem;
	public string previousQuestBossSuccessful;
	public string previousQuestBossFailed;
	private Item rewardItem;
	private int rewardCoins;
	private string monsterAction;
	[HideInInspector]public Boss boss; 
	[HideInInspector]public Location location;
	[HideInInspector]public Moment moment;
	

	public Battle battle{
		get{
			if(_battle == null){

				float t = Time.time;
				_battle = new Battle(t.ToString(),location.locationName,moment.momentName,location.locationBackground,moment.momentColor, averageArray(location.monsterChances,moment.monsterChances));
				FightManager.battles.Add(t.ToString(),_battle);

			}
			return _battle;
		}
	}
	public int numberOfEnemies;

	private Battle _battle;
				                 
	private float[] averageArray(float[] f1,float[] f2){
		for (int i = 0; i<f1.Length; i++) {
			f1[i] = (f1[i] + f2[i]);
		}
		return f1;
	}
	public string calculateQuestString (){
		string s = "";
		if (DramaManager.numberOfQuests == 0) {
			s = firstQuest;
			s = ReplaceTags (s);
		} else {
			if (this.questState == QuestState.WaitingRevange) {
				if (DramaManager.previousQuest.questType == QuestType.ItemQuest && DramaManager.previousQuest.questState == QuestState.Completed) {
					s = revangeQuestWithItem;
					s = ReplaceTags (s);
			
				} else {
					s = revangeQuestWithoutItem;
					s = ReplaceTags (s);
				}
			} else if (DramaManager.previousQuest != null && DramaManager.previousQuest.questState == QuestState.Completed) {
				if (DramaManager.previousQuest != null && DramaManager.previousQuest.questType == QuestType.MinibossQuest) {
					s = previousQuestBossSuccessful;
					s = ReplaceTags (s);

				} else {
					if (DramaManager.hasMiniBoss)
						s = previousQuestSuccesfulMiniBossKilled;
					else
						s = previousQuestSuccesfulNoMiniBoss;
					s = ReplaceTags (s);
				}

			} else {
				if (DramaManager.previousQuest != null && (DramaManager.previousQuest.questType == QuestType.MinibossQuest || DramaManager.previousQuest.questType == QuestType.EndBossQuest)) {
					s = previousQuestBossFailed;
					s = ReplaceTags (s);

				
		
				} else {
					s = previousQuestFailedNoMiniBoss;
					if (DramaManager.hasMiniBoss)
						s = previousQuestFailedMiniBossKilled;
					else
						s = previousQuestFailedNoMiniBoss;
					s = ReplaceTags (s);
				}



			}
		}
		return s;
	}
	private string ReplaceTags(string s){
		if (DramaManager.MiniBossQuest != null) {
			s = s.Replace ("[mba]", DramaManager.MiniBossQuest.monsterAction);
			s = s.Replace ("[mbl]", DramaManager.MiniBossQuest.location.locationName);
			s = s.Replace ("[mbt]", DramaManager.MiniBossQuest.moment.momentName);
			s = s.Replace ("[mbn]", DramaManager.MiniBossQuest.boss.name);
		}
		
		if (DramaManager.EndBossQuest != null) {
			s = s.Replace ("[eba]", DramaManager.EndBossQuest.monsterAction);
			s = s.Replace ("[ebl]", DramaManager.EndBossQuest.location.locationName);
			s = s.Replace ("[ebt]", DramaManager.EndBossQuest.moment.momentName);
			s = s.Replace ("[ebn]", DramaManager.EndBossQuest.boss.name);
		}
		
		if (DramaManager.previousQuest != null) {
			s = s.Replace ("[pr]", DramaManager.previousQuest.rewardName);
			s = s.Replace ("[pl]", DramaManager.previousQuest.location.locationName);
			s = s.Replace ("[pt]", DramaManager.previousQuest.moment.momentName);
			s = s.Replace ("[pa]", DramaManager.previousQuest.attacker);
		}
		s = s.Replace("[ct]",this.moment.momentName);
		s = s.Replace("[cl]",this.location.locationName);
		s = s.Replace("[a]", this.attacker);
		s = s.Replace ("[r]", this.rewardName);
		return s;
	}
	public void GenerateRandomQuest(){

		appearances++;
		if (this.questState == QuestState.Completed) {
			this.questState = QuestState.NotStarted;
		}
		if (this.possibleBossTypes != null && this.possibleBossTypes.Count > 0) {
			this.boss = DramaManager.bossList [possibleBossTypes [Mathf.Clamp((int)Random.Range (0, possibleBossTypes.Count),0,possibleBossTypes.Count-1)]];
			this.monsterAction = this.boss.GetRandomAction;
		} else {
			this.boss = null;
			this.monsterAction = "";
		}

		this.location = DramaManager.locationList [possibleLocations [Mathf.Clamp((int)Random.Range (0, possibleLocations.Count),0,possibleLocations.Count-1)]];
		if(DramaManager.previousQuest != null){
			while(this.location.locationName == DramaManager.previousQuest.location.locationName){
				this.location = DramaManager.locationList [possibleLocations [Mathf.Clamp((int)Random.Range (0, possibleLocations.Count),0,possibleLocations.Count-1)]];
			}
		}
		this.moment = DramaManager.momentList[possibleMoments[Mathf.Clamp((int)Random.Range (0, possibleMoments.Count),0,possibleMoments.Count-1)]];
		if (questType == QuestType.ItemQuest) {
			switch (giverTrait) {
			case TraitManager.TraitTypes.Magic:
				rewardItem = new Item (Item.ItemType.Magic, Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (2 * HubManager.shop.level, 5 * HubManager.shop.level), location.GetRandomRewardName ());
				break;
			case TraitManager.TraitTypes.Archery:
				rewardItem = new Item (Item.ItemType.Ranged, Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (2 * HubManager.shop.level, 5 * HubManager.shop.level), location.GetRandomRewardName ());
				break;
			case TraitManager.TraitTypes.Melee:
				rewardItem = new Item (Item.ItemType.Melee, Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), (int)Random.Range (.3f * HubManager.shop.level, .8f * HubManager.shop.level), location.GetRandomRewardName ());
				break;
			case TraitManager.TraitTypes.Armory:
				rewardItem = new Item (Item.ItemType.Armor, Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (2 * HubManager.shop.level, 5 * HubManager.shop.level), location.GetRandomRewardName ());
				break;
			}
		} else if (questType == QuestType.GoldQuest) {
			rewardCoins = 500 * Mathf.Max(1,GameData.TurnNumber);
		}
	}
	public float Fittness{
		get{
			float correctType = 0;
			if(this == DramaManager.previousQuest){
					correctType = -3;
			}else
			if(DramaManager.hasFarmUpgrade && DramaManager.hasItemQuest && DramaManager.hasMiniBoss && DramaManager.hasShopUpgrade){
				if(this.questType == QuestType.EndBossQuest) correctType = 1;
				else correctType = 0;
			}else if(DramaManager.hasFarmUpgrade && DramaManager.hasItemQuest && DramaManager.hasMiniBoss){
				if(this.questType == QuestType.ShopQuest) correctType = 1;
				else correctType = 0;
			}else if(DramaManager.hasFarmUpgrade && DramaManager.hasItemQuest){
				if(this.questType == QuestType.MinibossQuest) correctType = 1;
				else correctType = 0;
			}else if(DramaManager.hasFarmUpgrade){
				if(this.questType == QuestType.ItemQuest) correctType = 1;
				else correctType = 0;
			}else{
				if(this.questType == QuestType.FarmQuest) correctType = 1; else
				if(this.questType == QuestType.ShopQuest) correctType = 1; else
				
				if(DramaManager.numberOfQuests == 0){
					if(this.questType == QuestType.ItemQuest || this.questType == QuestType.EndBossQuest || this.questType == QuestType.MinibossQuest) correctType = -10;
					else correctType = 1;
				}
				else correctType = -0.1f;
			}
			if(this.questState == QuestState.Completed){
				correctType -= 3;
			}else if(this.questState == QuestState.Failed){
				correctType -=1;
			}else if(this.questState == QuestState.WaitingRevange){
				correctType +=0.2f;
			}
			float goodMorale = 0;
			List<Character> CharactersWithTrait  = CharacterManager.charactersByTrait[giverTrait];
			foreach(Character c in CharactersWithTrait){
				if(c.CalculateMoral()>goodMorale){
					goodMorale = c.CalculateMoral();
					questGiver = c;
				}
			}
			float fewAppearances = 1 - ((this.appearances - DramaManager.minAppearances ) / Mathf.Max(1,(DramaManager.maxAppearances - DramaManager.minAppearances)));
			float v =1-((correctType+goodMorale+fewAppearances)/3);
			Debug.Log("Quest Fittness for "+questType+" "+giverTrait+":"+v );
			return  v;
		}
	}
	public void FinishQuest(bool success){
		if (success) {
			ApplyReward ();

			if (questType == QuestType.EndBossQuest) {
				Application.LoadLevel(1);
			}else{
				questState = QuestState.Completed;
			}
		}else{
			if(questType == QuestType.EndBossQuest || questType == QuestType.MinibossQuest){
				questState = QuestState.WaitingRevange;
			}else questState = QuestState.Failed;
		}
	}
	private void ApplyReward(){
		if (this.questType == QuestType.ItemQuest) {
			InventoryManager.ItemsInInventory.Add (rewardItem);
		} else if (this.questType == QuestType.GoldQuest) {
			GameData.NumberOfCoins += rewardCoins;
		} else if (this.questType == QuestType.FarmQuest) {
			HubManager.farm.farmLevel ++;
		} else if (this.questType == QuestType.ShopQuest) {
			HubManager.shop.level++;
		}

	}


}
