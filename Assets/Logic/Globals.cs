using UnityEngine;

public delegate void VOID_FUNCTION(); 
public delegate void VOID_FUNCTION_CHARACTER(Character c);
public delegate void VOID_FUNCTION_VECTOR2(Vector2 vector);
public delegate void VOID_FUNCTION_VECTOR2_CALLBACK(Vector2 v, VOID_FUNCTION_CHARACTER callback);
public struct AnimationNames{
	public const string kBowAttack = "BowShot";
	public const string kMagicAttack = "MagicAttack";
	public const string kSwordAttack = "SwordAttack";
	public const string kWalk = "Walk";
}
public enum GameScenes{
	Hub,
	Fight
}