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
		//TODO ADD THE BOSS IN THE BATTLE
		if (DramaPack.DramaManager.activeQuest.qd.isBossQuest) {
			mBoss ++;
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
			yield return new WaitForSeconds(.1f+Random.Range(0f,3f));
		}
		if (DramaPack.DramaManager.activeQuest.qd.isBossQuest) {
			Vector3 mPos = minPos;
			Vector3 MPos = maxPos;
			Vector3 randomSpawnPos = new Vector3(mPos.x + Random.Range(0,MPos.x - mPos.x),
			                                     mPos.y + Random.Range(0,MPos.y - mPos.y),
			                                     mPos.z);
			LevelEnemies[number] = Instantiate(DramaPack.DramaManager.nextBoss.bossObject,randomSpawnPos,Quaternion.identity) as Enemy;
			LevelEnemies[number].GenerateEnemy(true);
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
