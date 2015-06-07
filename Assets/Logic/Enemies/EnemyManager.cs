using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	private static EnemyManager s_Instance;
	[SerializeField] Transform bossSpawnPosition;

	public static Enemy[] LevelEnemies;
	private void Awake(){
		s_Instance = this;
	}


	public static void GenerateEnemies(int number, Vector3 minPos, Vector3 maxPos, Enemy[] enemyTypes){
		s_Instance.StartCoroutine (s_Instance.SpawnEnemiesDelayed (number,minPos,maxPos,enemyTypes));
	}
	private IEnumerator SpawnEnemiesDelayed(int number, Vector3 minPos, Vector3 maxPos, Enemy[] enemyTypes){
		number +=(int)Random.Range(GameData.Progression/2,GameData.Progression);
		number = Mathf.Clamp (number, 5, 25);
		float[] typeChance = FightManager.battles[GameData.nextBattleID].monsterSpawnChances;


		int mBoss = 0;
		
		if ((DramaManager.currentQuest != null && (DramaManager.currentQuest.questType == Quest.QuestType.MinibossQuest || DramaManager.currentQuest.questType == Quest.QuestType.EndBossQuest) )||
		    (StaticDramaManager.currentQuest!= null && StaticDramaManager.currentQuest.bossPrefab!=null)) {
			mBoss=1;
		}
		
		LevelEnemies = new Enemy[number+mBoss];
		for(int i = 0; i<number; i++){

			int eType = 0;
			float totalChance = typeChance[eType];
			float rChance = Random.value;
			while(rChance>totalChance){
			
				totalChance += typeChance[(++eType)%4];
			}
			Vector3 mPos = minPos;
			Vector3 MPos = maxPos;
			Vector3 randomSpawnPos = new Vector3(mPos.x + Random.Range(0,MPos.x - mPos.x),
			                                     mPos.y + Random.Range(0,MPos.y - mPos.y),
			                                     mPos.z);
			LevelEnemies[i] = Instantiate(enemyTypes[eType%4],randomSpawnPos,Quaternion.identity) as Enemy;
			LevelEnemies[i].GenerateEnemy();
			yield return new WaitForSeconds(.1f);
		}
		if (mBoss == 1) {
			if(StaticDramaManager.currentQuest!= null && StaticDramaManager.currentQuest.bossPrefab!=null){
				Vector3 randomSpawnPos = new Vector3(minPos.x + Random.Range(0,maxPos.x - minPos.x),
				                                     minPos.y + Random.Range(0,maxPos.y - minPos.y),
				                                     minPos.z + Random.Range(0,maxPos.z - minPos.z));
				LevelEnemies[number] = Instantiate(StaticDramaManager.currentQuest.bossPrefab,s_Instance.bossSpawnPosition.position,Quaternion.identity) as Enemy;
				
				LevelEnemies[number].GenerateEnemy();
				Debug.Log("eb");
			}else{
				Vector3 randomSpawnPos = new Vector3(minPos.x + Random.Range(0,maxPos.x - minPos.x),
				                                     minPos.y + Random.Range(0,maxPos.y - minPos.y),
				                                     minPos.z + Random.Range(0,maxPos.z - minPos.z));
				LevelEnemies[number] = Instantiate(DramaManager.currentQuest.boss.prefab,s_Instance.bossSpawnPosition.position,Quaternion.identity) as Enemy;
				
				LevelEnemies[number].GenerateEnemy();
				Debug.Log("mb");
			}
			
		}
	}
	public static void CleanUP(){
		if (LevelEnemies != null) {
			for(int i =0; i<LevelEnemies.Length; i++){
				if(LevelEnemies[i]!=null)Destroy(LevelEnemies[i].gameObject);
			}
		}
		LevelEnemies = null;
	}
}
