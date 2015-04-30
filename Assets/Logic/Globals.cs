using UnityEngine;

public delegate void VOID_FUNCTION(); 
public delegate void VOID_FUNCTION_CHARACTER(Character c);
public delegate void VOID_FUNCTION_ENEMY(Enemy e);
public delegate void VOID_FUNCTION_VECTOR2(Vector2 vector);
public delegate void VOID_FUNCTION_VECTOR2_CALLBACK(Vector2 v, VOID_FUNCTION_CHARACTER callback);

public class AnimationNames{
	public const string kBowAttack = "BowShot";
	public const string kMagicAttack = "MagicAttack";
	public const string kSwordAttack = "SwordAttack";
	public const string kWalk = "Walk";
	public const string kEnemyAttack = "Attack";
}
public enum MonsterTypes{
	Wizard,
	Pirate,
	Devil
}
public class ColorCodes{
	private static Color[] _tint = new Color[3]{
		getColor(124,106,106),
		getColor(117,39,124),
		getColor(124,118,39)
	};
	private static float[][,] _colorRandomConfig = new float[3][,]{
		//Wizard
		new float[3,2]{
			{.6f,1},{.6f,1},{.0f,.7f}
		},
		//Pirate
		new float[3,2]{
			{.6f,1},{.6f,1},{.0f,.5f}
		},
		//Devil
		new float[3,2]{
			{.9f,1},{0f,.1f},{0.02f,.3f}
		}
	};
	private static Color getColor(int r,int g, int b, int a = 255){
		return new Color ((float)r / 255, (float)g / 255, (float)b / 255, (float)a / 255);
	}
	public static float[,] getColorConfig(MonsterTypes monsterType){
		return _colorRandomConfig [(int)monsterType];
	}
	public static Color tint{
		get{
			return _tint[(int)Random.Range(0,3)];
		}
	}
}
public enum GameScenes{
	None,
	Hub,
	Fight
}
public enum FightState
{
	Idle,
	MovingToBattle,
	Waiting,
	Attack,
	StandGround,
	Fallback,
	Flee
}
