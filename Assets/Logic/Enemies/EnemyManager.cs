using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	private static EnemyManager s_Instance;
	[SerializeField] Enemy[] enemyTypes;
	[SerializeField] Transform minSpawn, maxSpawn;
	public static Enemy[] LevelEnemies;
	private void Awake(){
		s_Instance = this;
	}
	public static void GenerateEnemies(float[] typeChance, int number){
		LevelEnemies = new Enemy[number];
		for(int i = 0; i<number; i++){
			int eType = 0;
			float totalChance = typeChance[eType];
			float rChance = Random.value;
			while(rChance>totalChance){
				totalChance += typeChance[++eType];
			}
			Vector3 mPos = s_Instance.minSpawn.position;
			Vector3 MPos = s_Instance.maxSpawn.position;
			Vector3 randomSpawnPos = new Vector3(mPos.x + Random.Range(0,MPos.x - mPos.x),
			                                     mPos.y + Random.Range(0,MPos.y - mPos.y),
			                                     mPos.z + Random.Range(0,MPos.z - mPos.z));
			LevelEnemies[i] = Instantiate(s_Instance.enemyTypes[eType],randomSpawnPos,Quaternion.identity) as Enemy;
			LevelEnemies[i].GenerateEnemy();
		}
	}
}
