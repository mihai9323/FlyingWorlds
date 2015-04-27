using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {
	private static int hashWalk, hashMagicAtt, hashSwordAtt, hashBowAtt;
	// Use this for initialization
	void Awake () {
		hashWalk = Animator.StringToHash (AnimationNames.kWalk);
		hashMagicAtt = Animator.StringToHash (AnimationNames.kMagicAttack);
		hashSwordAtt = Animator.StringToHash (AnimationNames.kSwordAttack);
		hashBowAtt = Animator.StringToHash (AnimationNames.kBowAttack);
	}
	
	public static int AnimationStringToID(string animation){
		switch(animation){
		case AnimationNames.kBowAttack: return hashBowAtt;
		case AnimationNames.kSwordAttack: return hashSwordAtt;
		case AnimationNames.kMagicAttack: return hashMagicAtt;
		case AnimationNames.kWalk: return hashWalk;
		default: return Animator.StringToHash(animation);
		}
	}
}
