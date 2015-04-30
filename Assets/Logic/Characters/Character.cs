﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character : MonoBehaviour {


	public FightState fightState;
	public AI ai;
	public string Name;
	public TraitManager.TraitTypes[] Traits;
	public Skillset Skills;
	public CharacterLooks Looks;
	public Portrait CharacterPortrait;

	public float movementSpeed = 1.0f;
	public float fleeSpeed = 1.5f;
	public bool inFightingParty;
	public bool fled;

	public Item WeaponItem{
		get{
			if(weaponItem == null) return new Item();
			return weaponItem;
		}
		set{
			weaponItem = value;
			Looks.SetActiveWeapon(value);

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
	private int health;
	public int Health {
		get{ return health;}
		set {
			health = (int)Mathf.Clamp(value,0,MaxHealth);
		}

	}
	public int MaxHealth{
		get{
			
			return (int)(Level * (TraitManager.GetTrait(Traits[0]).healthBonus + TraitManager.GetTrait(Traits[1]).healthBonus + 20));
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
				return (int)(((Level + Skills.magic) * (TraitManager.GetTrait(Traits[0]).magicBonus + TraitManager.GetTrait(Traits[1]).magicBonus)+1) * WeaponItem.Damage );			
			}else if(WeaponItem.itemType == Item.ItemType.Melee){
				return (int)(((Level + Skills.melee)*( TraitManager.GetTrait(Traits[0]).meleeBonus + TraitManager.GetTrait(Traits[1]).meleeBonus) +1) * WeaponItem.Damage );			
			}else if(WeaponItem.itemType == Item.ItemType.Ranged){
				return (int)(((Level + Skills.archery )*( TraitManager.GetTrait(Traits[0]).rangedBonus + TraitManager.GetTrait(Traits[1]).rangedBonus) + 1) * WeaponItem.Damage);			
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
	private Transform movement_target_transform;
	private void Awake(){

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
		Health = MaxHealth;
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

	public void Hit(int damage){

		Health -= Mathf.Max (damage-Armor,1);
		if (Health <= 1) {
			fightState = FightState.Flee;

			this.tag = "Dead";
		}
	}

	public void MoveCharacterToPosition(Vector2 position, VOID_FUNCTION_CHARACTER callback){
		movement_target = position;
		current_speed =  (fightState == FightState.Attack || fightState == FightState.Idle)? movementSpeed : fleeSpeed;
		Looks.StartAnimation (AnimationNames.kWalk);
		StopCoroutine("MoveToPosition");
		StopCoroutine ("MoveToTransform");
		StartCoroutine("MoveToPosition",callback);
	}


	public IEnumerator MoveToPosition(VOID_FUNCTION_CHARACTER callback){
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
			initZ = Mathf.Lerp(-10,0,Camera.main.WorldToScreenPoint(transform.position).y/Screen.height);
			transform.position = new Vector3(transform.position.x,transform.position.y,initZ);
			yield return null;
			remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		}
		Looks.StopAnimation ();
		if (callback != null)
			callback (this);

	}

	private void FaceTarget(){

		if(movement_target!=null)FaceDirection ((int)(movement_target.x-transform.position.x));

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
	public void FaceDirection(int direction){
		if (direction != 0) {
			Vector3 lScale = origScale;
			lScale.x = Mathf.Abs (lScale.x) * Mathf.Sign (direction);
			transform.localScale = lScale;
		}
	}
	public void StartAITick(){
		StartCoroutine ("AITick");
	}
	public void StopAITick(){

		if (this.gameObject.activeInHierarchy) {
			StopCoroutine ("AITick");

			
		}

	}

	public Enemy currentTarget;


	public void MoveCharacterToTransform(Transform trans, VOID_FUNCTION_CHARACTER callback){
		movement_target_transform = trans;
		current_speed = movementSpeed;
		Looks.StartAnimation (AnimationNames.kWalk);
		StopCoroutine("MoveToTransform");
		StopCoroutine("MoveToPosition");
		StartCoroutine("MoveToTransform",callback);
		
	}	
	public IEnumerator MoveToTransform(VOID_FUNCTION_CHARACTER callback){
		movement_target = movement_target_transform.position;
		float remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		float initialDistance = Vector2.Distance(this.transform.position,movement_target);
		Vector2 initialPosition = transform.position;
		float ct = 0;
		while (remainingDistance>weaponItem.Range*weaponItem.Range && movement_target_transform!=null) {
			FaceTarget();
			float initZ = transform.position.z;
			
			movement_target = movement_target_transform.position;
			
			
			
			
			this.transform.position += (-transform.position + (Vector3)movement_target).normalized * Time.deltaTime * movementSpeed;
			initZ = Mathf.Lerp(-10,0,Camera.main.WorldToScreenPoint(transform.position).y/Screen.height);
			transform.position = new Vector3(transform.position.x,transform.position.y,initZ);
			yield return null;
			remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		}
		Looks.StopAnimation ();
		if (callback != null)
			callback (this);
		
	}



	private void FindTargetAndAttack(){
		var target = FindTarget ();
			
		if (target == null) {
			StopFight ();
		} else {
			if (IsNewTarget (target)) {
				MoveAndAttack (target);
			} else if (IsSameTarget (target)) {
				if (IsInWeaponRange (target)) {
					Attack (target);						
				} else {
					MoveAndAttack (target);
				}
			}
		}
	}

	public void StandGround(){ 
		var target = FindTarget ();
		if (target == null) {
			StopFight ();
		} else {
			if (IsNewTarget (target)) {
				if (IsInWeaponRange (target, 1.5f)) {
					MoveAndAttack (target);
				} else {
					currentTarget = target;
				}
			} else if (IsSameTarget (target)) {
				if (IsInWeaponRange (target)) {
					Attack (target);
				} else if (IsInWeaponRange (target, 1.5f)) {
					MoveAndAttack (target);
				}
			}
		}
	}
	public void Flee(){
		fled = true;
		MoveCharacterToPosition (FightManager.FleeTarget.position, delegate(Character c) {
			StopAITick();
			FightManager.CheckLost();
		});
	}

	private void MoveAndAttack (Enemy targetCharacter)
	{
		currentTarget = targetCharacter;
		MoveCharacterToTransform (currentTarget.transform, delegate (Character c) {
			Attack (targetCharacter);
		});
	}
	private void Attack (Enemy targetCharacter)
	{
		if (targetCharacter != null) {
		
			Looks.StartAnimation (weaponItem.fightAnimation);
			FaceDirection((int)(targetCharacter.transform.position.x - this.transform.position.x));
			if(weaponItem.fightAnimation == AnimationNames.kBowAttack){
				ShootProjectile(FightManager.arrow,targetCharacter);
			}else if(weaponItem.fightAnimation == AnimationNames.kMagicAttack){
				ShootProjectile(FightManager.fireBall,targetCharacter);
			}else{
				targetCharacter.Hit (this.Damage);
			}

		}
		StopAllCoroutines ();
		Invoke ("StartAITick", 1.2f);

	}
	private void ShootProjectile(Projectile p, Enemy target){
		Projectile missile = Instantiate (p, transform.position, Quaternion.identity) as Projectile;
		missile.ShootMonster (target.transform.position, this.Damage, null);
	}
	private void StopFight ()
	{
		StopAITick ();
		Looks.StopAnimation ();
	}
	private Enemy FindTarget ()
	{
		Enemy targetCharacter = null;
		GameObject gob = GameObject.FindGameObjectsWithTag ("ENEMY").OrderBy (go => Vector3.SqrMagnitude (go.transform.position - transform.position)).FirstOrDefault ();
		if (gob != null && gob.GetComponent<Enemy> () != null)
			targetCharacter = gob.GetComponent<Enemy> ();
		return targetCharacter;
	}
	private bool IsNewTarget (Enemy targetCharacter)
	{
		return targetCharacter != null && currentTarget != targetCharacter;
	}
	private bool IsSameTarget (Enemy targetCharacter)
	{
		return targetCharacter == currentTarget;
	}
		
	private bool IsInWeaponRange (Enemy target, float percent = 1.0f)
	{
		return CustomSqrDistance (this.transform.position, target.transform.position) < weaponItem.Range * weaponItem.Range * percent;
	}

	public void FallBack(){
		MoveCharacterToPosition (FightManager.FleeTarget.position, delegate(Character c) {
			fightState = FightState.Flee;
		});
		fightState = FightState.Fallback;
	}
	private IEnumerator AITick(){
		while (!fled) {
			Tick ();
			yield return new WaitForSeconds(.5f + Random.value*.5f);
		}
	}
	public void Tick(){
		if (!fled) {
			switch (fightState) {
			case FightState.Attack:
				FindTargetAndAttack ();
				break;
			case FightState.Fallback:
				FallBack ();
				break;
			case FightState.StandGround:
				StandGround ();
				break;
			case FightState.Flee:
				Flee ();
				break;
			}
		}
	}
	public void CleanUpAfterFight(){
		this.StopAllCoroutines ();
		this.currentTarget = null;
		this.fightState = FightState.Idle;
		this.Looks.StopAnimation ();
		this.transform.position = HubManager.road.RoadWorldSpace.position;
		RemoveFromParty ();
	}
}
