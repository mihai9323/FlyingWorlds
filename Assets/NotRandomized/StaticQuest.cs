using UnityEngine;
using System.Collections;
[System.Serializable]
public class StaticQuest {

	[TextArea(3,10)]public string questString;
	[SerializeField] Battle _battle;
	public Battle battle{
		get{
			if(!FightManager.battles.ContainsKey(_battle.id)) FightManager.battles.Add(_battle.id,_battle);
			return _battle;

			
		}
	}
	public Enemy bossPrefab;
	public Character questGiver;
	public Quest.QuestType questType;

	public Item itemReward;

	public void ApplyReward(){
		switch (questType) {
		case Quest.QuestType.ItemQuest: InventoryManager.ItemsInInventory.Add(itemReward); break;
		case Quest.QuestType.FarmQuest: HubManager.farm.farmLevel++; break;
		case Quest.QuestType.ShopQuest: HubManager.shop.level++; break;
		}
	}
}
