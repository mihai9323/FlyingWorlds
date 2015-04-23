using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

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
	[HideInInspector]public FightState fightState;
	public string Name;
	public TraitManager.TraitTypes[] Traits;
	public Skillset Skills;
	public CharacterLooks Looks;
	public Portrait CharacterPortrait;

	public float movementSpeed = 1.0f;
	public float fleeSpeed = 1.5f;
	public bool inFightingParty;
	public Item WeaponItem{
		get{
			if(weaponItem == null) return new Item();
			return weaponItem;
		}
		set{
			Looks.SetActiveWeapon(value);
			weaponItem = value;
		}
	}
	
	public Item ArmorItem{
		get{
			if(armorItem == null) return new Item();
			return armorItem;
		}
		set{
			Looks.SetActiveArmor(value);
			armorItem = value;
		}
	}
	
	[SerializeField] Item weaponItem, armorItem;
	
	public int Level;
	public LabelManager.LabelType[] Labels;
	public bool Ready;
	
	public string Thinks{
		get{
			return iThink();
		}	
	}
	
	public int MaxHealth{
		get{
			
			return (int)(Level * (TraitManager.GetTrait(Traits[0]).healthBonus + TraitManager.GetTrait(Traits[1]).healthBonus + 1));
		}
	}
	public int Armor {
		get{
			return (int)(ArmorItem.Defence * (TraitManager.GetTrait(Traits[0]).armorBonus + TraitManager.GetTrait(Traits[1]).armorBonus + 1));
		}
	}
	public int Damage{
		get{
			if(WeaponItem.itemType == Item.ItemType.Magic){
				return (int)(((Level + Skills.magic) * (TraitManager.GetTrait(Traits[0]).magicBonus + TraitManager.GetTrait(Traits[1]).magicBonus)+1) * WeaponItem.Damage * WeaponItem.Speed);			
			}else if(WeaponItem.itemType == Item.ItemType.Melee){
				return (int)(((Level + Skills.melee)*( TraitManager.GetTrait(Traits[0]).meleeBonus + TraitManager.GetTrait(Traits[1]).meleeBonus) +1) * WeaponItem.Damage * WeaponItem.Speed);			
			}else if(WeaponItem.itemType == Item.ItemType.Ranged){
				return (int)(((Level + Skills.archery )*( TraitManager.GetTrait(Traits[0]).rangedBonus + TraitManager.GetTrait(Traits[1]).rangedBonus) + 1) * WeaponItem.Damage * WeaponItem.Speed);			
			}else return (int)(((Level + Skills.melee )*( TraitManager.GetTrait(Traits[0]).meleeBonus + TraitManager.GetTrait(Traits[1]).meleeBonus) + 1));			
		}
	}
	public int Moral{
		get{
			return (int)Mathf.Clamp ((int)(TraitManager.GetTrait(Traits[0]).moraleBonus + TraitManager.GetTrait(Traits[1]).moraleBonus) + CalculateMoral(),0,100);
		}
	}

	private Vector2 movement_target;
	private float current_speed;
	private Vector3 origScale;
	private Character character;
	private void Start(){

		origScale = transform.localScale;
	}
	private void OnDestroy(){

	}

	public float CalculateMoral(){
		return 0.5f;
	}
	
	
	public void CreateCharacter(){
		Level = 1;
		Traits = TraitManager.GetRandomTraitsTypes(2);
		Skills.CreateSkillset();
		Name = CharacterManager.GenerateCharacterName();
		Looks.GenerateLooks ();
		Ready = true;

	}
	
	private string iThink(){
		string message="";
		List<LabelManager.LabelType> accounted = new List<LabelManager.LabelType>(); 
		foreach(Character c in CharacterManager.gameCharacters){
			foreach(LabelManager.LabelType l in c.Labels){
				
				Trait t1 = TraitManager.GetTrait(this.Traits[0]);
				Trait t2 = TraitManager.GetTrait(this.Traits[1]);
				
				if(System.Array.IndexOf(t1.hateSelfLabel, l)>-1 || System.Array.IndexOf(t2.hateSelfLabel, l) >-1 && !accounted.Contains(l)){
				   message += LabelManager.GetLabel(l).DontLikeSelfString()+"\n";	
				   accounted.Add(l);
				}
				if(System.Array.IndexOf(t1.hateOtherLabel, l)>-1 || System.Array.IndexOf(t2.hateOtherLabel, l) >-1 && !accounted.Contains(l)){
					message += LabelManager.GetLabel(l).DontLikeOtherString("")+"\n";
					accounted.Add(l);	
				}
				
				
			}
		}
		return message;
	}

	public void MoveCharacterToPosition(Vector2 position){
		movement_target = position;
		current_speed =  (fightState == FightState.Attack || fightState == FightState.Idle)? movementSpeed : fleeSpeed;
		Looks.StartAnimation (AnimationNames.kWalk);
		StopCoroutine("MoveToPosition");
		StartCoroutine("MoveToPosition", null);

	}
	public void MoveCharacterToPosition(Vector2 position, VOID_FUNCTION_CHARACTER callback){
		movement_target = position;
		current_speed =  (fightState == FightState.Attack || fightState == FightState.Idle)? movementSpeed : fleeSpeed;
		Looks.StartAnimation (AnimationNames.kWalk);
		StopCoroutine("MoveToPosition");
		StartCoroutine("MoveToPosition",callback);
		
	}


	private IEnumerator MoveToPosition(VOID_FUNCTION_CHARACTER callback){
		float remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		float initialDistance = Vector2.Distance(this.transform.position,movement_target);
		Vector2 initialPosition = transform.position;
		float ct = 0;
		while (remainingDistance>.2f) {
			FaceTarget();
			float initZ = transform.position.z;
			ct += movementSpeed * Time.deltaTime /initialDistance;
			this.transform.position = Vector2.Lerp(initialPosition, 
			                                       movement_target, 
			                                       ct
			                                       );
			transform.position = new Vector3(transform.position.x,transform.position.y,initZ);
			yield return null;
			remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		}
		if (callback != null)
			callback (this);
		Looks.StopAnimation ();
	}

	private void FaceTarget(){
		if(transform.position.x>movement_target.x) transform.localScale = new Vector3(-origScale.x,origScale.y,origScale.z);
		else  transform.localScale = origScale;
	}
	private float CustomSqrDistance(Vector3 myPosition, Vector2 targetPosition){
			Vector2 auxPos = new Vector2 (myPosition.x, myPosition.y);
			return Vector2.SqrMagnitude(targetPosition - auxPos);
		}
	public void AddToParty(){
		if (!CharacterManager.partyFull) {
			if(!this.inFightingParty){
				this.inFightingParty = true;

			}else Debug.LogWarning("Allready in party");
		} else
			Debug.Log ("Sorry full party");
	}
	public void RemoveFromParty(){
		if(this.inFightingParty){
			this.inFightingParty = false;

		}else Debug.LogWarning("Allready removed from party");
	}
}
