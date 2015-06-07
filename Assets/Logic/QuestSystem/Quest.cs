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
		WaitingRevenge
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
	[TextArea(3,10)][SerializeField] string firstQuest;
	[TextArea(3,10)][SerializeField] string previousQuestSuccesfulNoMiniBoss;
	[TextArea(3,10)][SerializeField] string previousQuestFailedNoMiniBoss;
	[TextArea(3,10)][SerializeField] string previousQuestSuccesfulMiniBossKilled;
	[TextArea(3,10)][SerializeField] string previousQuestFailedMiniBossKilled;

	[TextArea(3,10)][SerializeField] string revangeQuestWithItem;
	[TextArea(3,10)][SerializeField] string revangeQuestWithoutItem;
	[TextArea(3,10)][SerializeField] string previousQuestBossSuccessful;
	[TextArea(3,10)][SerializeField] string previousQuestBossFailed;
	private Item rewardItem;
	private int rewardCoins;
	public string monsterAction;
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
			if (this.questState == QuestState.WaitingRevenge) {
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
					if (DramaManager.hasMiniBossQuest)
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
					if (DramaManager.hasMiniBossQuest)
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
		if (DramaManager.previousQuest != null) {
			s = s.Replace ("[pr]", string.IsNullOrEmpty(DramaManager.previousQuest.rewardName)? "experience":DramaManager.previousQuest.rewardName);
			s = s.Replace ("[pl]", DramaManager.previousQuest.location.locationName);
			s = s.Replace ("[pt]", DramaManager.previousQuest.moment.momentName);
			s = s.Replace ("[pa]", string.IsNullOrEmpty(DramaManager.previousQuest.attacker)? "enemies":DramaManager.previousQuest.attacker);
		}
		s = s.Replace("[ct]",this.moment.momentName);
		s = s.Replace("[cl]",this.location.locationName);
		s = s.Replace("[a]", this.attacker);
		s = s.Replace ("[r]", this.rewardName);
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
		if(!DramaManager.hasMiniBossQuest && DramaManager.MiniBossQuest!=null){
			s = s.Replace ("[pba]", DramaManager.MiniBossQuest.monsterAction);
			s = s.Replace ("[pbl]", DramaManager.MiniBossQuest.location.locationName);
			s = s.Replace ("[pbt]", DramaManager.MiniBossQuest.moment.momentName);
			s = s.Replace ("[pbn]", DramaManager.MiniBossQuest.boss.name);
		}else if(DramaManager.EndBossQuest!=null){
			s = s.Replace ("[pba]", DramaManager.EndBossQuest.monsterAction);
			s = s.Replace ("[pbl]", DramaManager.EndBossQuest.location.locationName);
			s = s.Replace ("[pbt]", DramaManager.EndBossQuest.moment.momentName);
			s = s.Replace ("[pbn]", DramaManager.EndBossQuest.boss.name);
		}
		return s;
	}
	public void GenerateQuest(){

		appearances++;
		if (this.questType == QuestType.EndBossQuest || this.questType == QuestType.MinibossQuest) {
			LoadBossQuest();
		} else {
			GenerateRandomQuest ();
		}
	}
	public void GenerateBossQuest(){
		GenerateRandomQuest ();
	}
	void LoadBossQuest(){
		if (this.questType == QuestType.EndBossQuest) {
			this.location = DramaManager.EndBossQuest.location;
			this.boss = DramaManager.EndBossQuest.boss;
			this.moment = DramaManager.EndBossQuest.moment;
			this.monsterAction = DramaManager.EndBossQuest.monsterAction;

		} else if (this.questType == QuestType.MinibossQuest) {
			this.location = DramaManager.MiniBossQuest.location;
			this.boss = DramaManager.MiniBossQuest.boss;
			this.moment = DramaManager.MiniBossQuest.moment;
			this.monsterAction = DramaManager.MiniBossQuest.monsterAction;
		}
	}
	void GenerateRandomQuest ()
	{
		if (this.questState == QuestState.Completed) {
			this.questState = QuestState.NotStarted;
		}
		if (this.possibleBossTypes != null && this.possibleBossTypes.Count > 0) {
			this.boss = DramaManager.bossList [possibleBossTypes [Mathf.Clamp ((int)Random.Range (0, possibleBossTypes.Count), 0, possibleBossTypes.Count - 1)]];
			this.monsterAction = this.boss.GetRandomAction;
		}
		else {
			this.boss = null;
			this.monsterAction = "";
		}
		this.location = DramaManager.locationList [possibleLocations [Mathf.Clamp ((int)Random.Range (0, possibleLocations.Count), 0, possibleLocations.Count - 1)]];
		if (DramaManager.previousQuest != null) {

			while (this.location.locationName == DramaManager.previousQuest.location.locationName) {
				this.location = DramaManager.locationList [possibleLocations [Mathf.Clamp ((int)Random.Range (0, possibleLocations.Count), 0, possibleLocations.Count - 1)]];
			}
		}
		this.moment = DramaManager.momentList [possibleMoments [Mathf.Clamp ((int)Random.Range (0, possibleMoments.Count), 0, possibleMoments.Count - 1)]];
		if (questType == QuestType.ItemQuest) {
			switch (giverTrait) {
			case TraitManager.TraitTypes.Magic:
				rewardItem = new Item (Item.ItemType.Magic, Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), location.GetRandomRewardName ());
				break;
			case TraitManager.TraitTypes.Archery:
				rewardItem = new Item (Item.ItemType.Ranged, Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), location.GetRandomRewardName ());
				break;
			case TraitManager.TraitTypes.Melee:
				rewardItem = new Item (Item.ItemType.Melee, Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), (int)Random.Range (.3f * HubManager.shop.level, .8f * HubManager.shop.level), location.GetRandomRewardName ());
				break;
			case TraitManager.TraitTypes.Armory:
				rewardItem = new Item (Item.ItemType.Armor, Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), Random.Range (12 * HubManager.shop.level, 15 * HubManager.shop.level), location.GetRandomRewardName ());
				break;
			}
		}
		else
			if (questType == QuestType.GoldQuest) {
				rewardCoins = 500 * Mathf.Max (1, GameData.Progression);
			}
	}

	public float Fitness{
		get{
			float correctType = 0;
			if(this == DramaManager.previousQuest){
					correctType = -3;
			}else
			if(DramaManager.hasFarmQuest && DramaManager.hasItemQuest && DramaManager.hasMiniBossQuest && DramaManager.hasShopQuest){
				if(this.questType == QuestType.EndBossQuest) correctType = 1;
				else correctType = 0;
			}else if((DramaManager.hasFarmQuest || DramaManager.hasShopQuest) && DramaManager.hasItemQuest){
				if(this.questType == QuestType.MinibossQuest) correctType = 3f;
				else if(this.questType == QuestType.FarmQuest || this.questType == QuestType.ShopQuest) correctType = .2f;
				else correctType = 0;
			}else if(DramaManager.hasFarmQuest && DramaManager.hasItemQuest){
				if(this.questType == QuestType.ShopQuest) correctType = 1;
				else correctType = 0;
			}else if(DramaManager.hasFarmQuest){
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
			}else if(this.questState == QuestState.WaitingRevenge){
				correctType +=0.2f;
			}
			if(!DramaManager.hasMiniBossQuest && this.questType == QuestType.EndBossQuest) correctType -= 10;
			if(DramaManager.hasMiniBossQuest && this.questType == QuestType.MinibossQuest) correctType -=10;
			if(this.questType == QuestType.MinibossQuest && DramaManager.previousQuest !=null && DramaManager.previousQuest.questType == QuestType.MinibossQuest) correctType -= 10;
			if(this.questType == QuestType.EndBossQuest && DramaManager.previousQuest !=null && DramaManager.previousQuest.questType == QuestType.EndBossQuest) correctType -= 10;
			float goodMorale = 0;
		
			if(CharacterManager.charactersByTrait.ContainsKey(giverTrait)){
				List<Character> CharactersWithTrait  = CharacterManager.charactersByTrait[giverTrait];
				foreach(Character c in CharactersWithTrait){
					if(c.CalculateMoral()>goodMorale){
						goodMorale = c.CalculateMoral();
						questGiver = c;
					}
				}
			}else{
				goodMorale = -10;
				questGiver = null;
			}

			float fewAppearances = 1 - ((this.appearances - DramaManager.minAppearances ) / Mathf.Max(1,(DramaManager.maxAppearances - DramaManager.minAppearances)));
			float v =1-((correctType+goodMorale+fewAppearances)/3);
			Debug.Log("Quest Fitness for "+questType+" "+giverTrait+":"+v );
			return  v;
		}
	}
	public void FinishQuest(bool success){
		if (success) {
			ApplyReward ();

			if (questType == QuestType.EndBossQuest) {
				Application.LoadLevel(Application.loadedLevel+1);
			}else{
				questState = QuestState.Completed;
			}
		}else{
			if(questType == QuestType.EndBossQuest || questType == QuestType.MinibossQuest){
				questState = QuestState.WaitingRevenge;
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
