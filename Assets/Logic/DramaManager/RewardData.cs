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
				if(_chosenItem == null) _chosenItem = new Item(rewardItems[(int)Random.Range(0,rewardItems.Length)],3);
				return _chosenItem;
			}
			set{
				_chosenItem = value;
			}
		}
		private Item _chosenItem;
		// Use this for initialization
		protected override void Update () {
#if UNITY_EDITOR
			if(this.rewardType == RewardType.Item){
				this.name = chosenItem.ItemName;
			}
#endif
			base.Update ();
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
				case RewardType.Treasure: GameData.NumberOfCoins+= DramaPack.DramaManager.progression * int.Parse(this.value.name); break;
				case RewardType.Progression: DramaPack.DramaManager.progression++; break;
				case RewardType.Item: InventoryManager.ItemsInInventory.Add (this.chosenItem); break;
			}
		}
		
		
	}
}