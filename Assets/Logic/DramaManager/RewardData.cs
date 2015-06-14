using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace DramaPack{
	public class RewardData : StringData {

		public enum RewardType{
			Progression,
			Treasure,
			Item,
			ShopUpgrade,
			FarmUpgrade
		}
		public RewardType rewardType;
		public Item[] rewardItems;

		public StringData value;
		public StrategyData strategy;


		[HideInInspector]public Item chosenItem{
			get{
				if(_chosenItem == null) _chosenItem = new Item(rewardItems[(int)Random.Range(0,rewardItems.Length)],Mathf.Clamp (DramaManager.progression+ HubManager.shop.level * 3,HubManager.shop.level * 6, HubManager.shop.level * 18));
				return _chosenItem;
			}
			set{
				_chosenItem = value;
			}
		}
		private Item _chosenItem;
		public string[] namesNonItem;
		[HideInInspector]public string chosenName{
			get{
				if(this.rewardType == RewardType.Item){
					return chosenItem.ItemName;
				}else
				if(string.IsNullOrEmpty(_chosenName)) _chosenName = namesNonItem[(int)Random.Range(0,namesNonItem.Length)];
				return _chosenName;
			}
			set{
				_chosenName = value;
			}
		}


		private string _chosenName;
		// Use this for initialization
		protected override void Update () {
#if UNITY_EDITOR
			if(this.rewardType == RewardType.Item){
				this.name = chosenItem.ItemName;
			}else{
				this.name = chosenName;
			}
#endif
			base.Update ();
		}
		public void GenerateName(){
			chosenItem = null;
			chosenName = "";
			if(this.rewardType == RewardType.Item){
				this.name = chosenItem.ItemName;
			}else{
				this.name = chosenName;
			}
		}
		public override string DisplayData(){
			List<TagReplacePair> pairs = new List<TagReplacePair> ();

			if (value != null)
				pairs.Add (new TagReplacePair ("[rVal]", value));
			if (strategy != null) {
				pairs.Add(new TagReplacePair("[rStr]", strategy));
			}

			return base.DisplayData (pairs.ToArray());
		}

		public void ApplyReward(){
			switch (this.rewardType) {
				case RewardType.FarmUpgrade: HubManager.farm.farmLevel++; break;
				case RewardType.ShopUpgrade: HubManager.shop.level++; break;
				case RewardType.Treasure: GameData.NumberOfCoins+= (int)(DramaPack.DramaManager.progression * int.Parse(this.value.name)); break;
				case RewardType.Progression:  DramaPack.DramaManager.nextBoss.status = BossData.Status.completed; DramaManager.CheckGameWin(); break;
				case RewardType.Item: InventoryManager.ItemsInInventory.Add (this.chosenItem); break;
			}
			chosenName = "";
			chosenItem = null;
		}
		
		
	}
}