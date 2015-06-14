using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {

	public static void Save(){
		HasDataStored = true;
		previousEndBossName = DramaPack.DramaManager.endBoss.name;
		previousMiniBossName = DramaPack.DramaManager.miniBoss.name;
		previousFarmLevel = HubManager.farm.farmLevel;
		previousShopLevel = HubManager.shop.level;
		previousItems = InventoryManager.ItemsInInventory.ToArray();
	}
	public static void ClearData(){
		HasDataStored = false;
		previousEndBossName = "";
		previousMiniBossName = "";
		previousFarmLevel = 1;
		previousShopLevel = 1;
		previousItems = new Item[0];
	}

	public static bool HasDataStored{
		get{
			return PlayerPrefs.GetInt("hasData",0) == 1;
		}
		set{
			PlayerPrefs.SetInt("hasData",value?1:0);
		}
	}

	public static string previousEndBossName{
		get{
			return PlayerPrefs.GetString("endBoss","");
		}
		set{
			PlayerPrefs.SetString("endBoss",value);
		}
	}
	public static string previousMiniBossName{
		get{
			return PlayerPrefs.GetString("miniBoss","");
		}
		set{
			PlayerPrefs.SetString("miniBoss",value);
		}	
	}
	public static int previousGameDataProgression{
		get{
			return PlayerPrefs.GetInt("gdP",1);
		}
		set{
			PlayerPrefs.SetInt("gdP",value);
		}	
	}
	public static int previousShopLevel{
		get{
			return PlayerPrefs.GetInt("slvl",1);
		}
		set{
			PlayerPrefs.SetInt("slvl",value);
		}	
	}
	public static int previousFarmLevel{
		get{
			return PlayerPrefs.GetInt("flvl",1);
		}
		set{
			PlayerPrefs.SetInt("flvl",value);
		}	
	}
	public static Item[] previousItems{
		get{
			int nrItems = PlayerPrefs.GetInt("nrItems",0);
			Item[] rVal = new Item[nrItems];
			for(int i =0 ; i<nrItems; i++){
				rVal[i] = RetrieveItem(i);
			}
			return rVal;
		}
		set{
			PlayerPrefs.SetInt("nrItems", value.Length);
			for(int i =0 ; i<value.Length; i++){
				StoreItem(value[i],i);
			}
		}
	}
	public static float startValue{
		get{
			float sVal = 0;
			foreach(Item i in previousItems){
				if(i.Damage+i.Defence>sVal) sVal = i.Damage+i.Defence;
			}
			return sVal;
		}
	}
	public static void StoreItem(Item item, int index){
		PlayerPrefs.SetInt ("iType" + index.ToString (), (int)item.itemType);
		PlayerPrefs.SetFloat ("iDmg" + index.ToString (), item.Damage);
		PlayerPrefs.SetFloat ("iRng" + index.ToString (), item.Range);
		PlayerPrefs.SetFloat ("iDef" + index.ToString (), item.Defence);
		PlayerPrefs.SetString ("iName" + index.ToString (), item.ItemName);

	}
	public static Item RetrieveItem(int index){
		Item.ItemType iType = (Item.ItemType)PlayerPrefs.GetInt ("iType" + index.ToString (), 0);
		float dmg = PlayerPrefs.GetFloat ("iDmg" + index.ToString (), 0);
		float rng = PlayerPrefs.GetFloat ("iRng" + index.ToString (), 0);
		float def = PlayerPrefs.GetFloat ("iDef" + index.ToString (), 0);
		string name = PlayerPrefs.GetString ("iName" + index.ToString (), "");

		return new Item (iType, (int)dmg, (int)def, (int)rng, name);
	}

}
