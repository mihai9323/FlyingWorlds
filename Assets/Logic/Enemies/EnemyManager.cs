using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	private static EnemyManager s_Instance;


	public static Enemy[] LevelEnemies;
	private void Awake(){
		s_Instance = this;
	}

	public static void GenerateEnemies(float[] typeChance, int number, Vector3 minPos, Vector3 maxPos, Enemy[] enemyTypes){
		LevelEnemies = new Enemy[number];
		for(int i = 0; i<number; i++){
			int eType = 0;
			float totalChance = typeChance[eType];
			float rChance = Random.value;
			while(rChance>totalChance){
				totalChance += typeChance[++eType];
			}
			Vector3 mPos = minPos;
			Vector3 MPos = maxPos;
			Vector3 randomSpawnPos = new Vector3(mPos.x + Random.Range(0,MPos.x - mPos.x),
			                                     mPos.y + Random.Range(0,MPos.y - mPos.y),
			                                     mPos.z + Random.Range(0,MPos.z - mPos.z));
			LevelEnemies[i] = Instantiate(enemyTypes[eType],randomSpawnPos,Quaternion.identity) as Enemy;
			LevelEnemies[i].GenerateEnemy();
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
