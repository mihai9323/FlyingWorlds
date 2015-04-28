using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character : MonoBehaviour {


	[HideInInspector]public FightState fightState;
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
		Vector3 lScale = origScale;
		lScale.x = Mathf.Abs (lScale.x) * Mathf.Sign (direction);
		transform.localScale = lScale;
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
			transform.position = new Vector3(transform.position.x,transform.position.y,initZ);
			yield return null;
			remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		}
		Looks.StopAnimation ();
		if (callback != null)
			callback (this);
		
	}


	private void FindTargetAndAttack(){
		Enemy targetCharacter = null;
		GameObject gob= GameObject.FindGameObjectsWithTag("ENEMY").OrderBy(go => Vector3.SqrMagnitude(go.transform.position- transform.position)).FirstOrDefault();
		if(gob!=null && gob.GetComponent<Enemy>()!=null)targetCharacter = gob.GetComponent<Enemy>();
			
			if (targetCharacter != null && currentTarget != targetCharacter) {
				
			currentTarget = targetCharacter;
			MoveCharacterToTransform (currentTarget.transform,
				                         delegate(Character c) {
				if (currentTarget != null)
					currentTarget.Hit (this.Damage);	
				StopAllCoroutines ();
				Invoke ("StartAITick", 1.2f);
					
				Looks.StartAnimation (weaponItem.fightAnimation);
				FaceTarget ();
			});
		} else if (targetCharacter == currentTarget) {
			if (currentTarget != null && CustomSqrDistance (this.transform.position, currentTarget.transform.position)<weaponItem.Range*weaponItem.Range){
					currentTarget.Hit (this.Damage);	
					StopAllCoroutines ();
					Invoke ("StartAITick", 1.2f);
					
					Looks.StartAnimation (weaponItem.fightAnimation);
					FaceTarget ();
				}
		}else if (targetCharacter == null) {
				StopAITick();
				Looks.StopAnimation();
			}
	


		
	}

	public void Flee(){
		fled = true;
		MoveCharacterToPosition (FightManager.FleeTarget.position, delegate(Character c) {
			StopAITick();
			FightManager.CheckLost();
		});
	}
	public void StandGround(){ 
		Enemy targetCharacter = null;
		GameObject gob= GameObject.FindGameObjectsWithTag("ENEMY").OrderBy(go => Vector3.SqrMagnitude(go.transform.position- transform.position)).FirstOrDefault();
		if(gob!=null && gob.GetComponent<Enemy>()!=null)targetCharacter = gob.GetComponent<Enemy>();
			if (targetCharacter!=null && targetCharacter != currentTarget) {
				currentTarget = targetCharacter;
				if(CustomSqrDistance(transform.position,currentTarget.transform.position)<weaponItem.Range*weaponItem.Range*3f/2){
				Looks.StartAnimation(AnimationNames.kWalk);
					MoveCharacterToTransform(currentTarget.transform,
					                         delegate(Character c) {
						currentTarget.Hit(this.Damage);	
						StopCoroutine("AITick");
						Invoke("StartAITick",1.2f);
						
						Looks.StartAnimation(weaponItem.fightAnimation);
						FaceTarget();
					});
				}else if (targetCharacter == currentTarget) {
				if (currentTarget != null && CustomSqrDistance (this.transform.position, currentTarget.transform.position)<weaponItem.Range*weaponItem.Range){
					currentTarget.Hit (this.Damage);	
					StopAllCoroutines ();
					Invoke ("StartAITick", 1.2f);
					
					Looks.StartAnimation (weaponItem.fightAnimation);
					FaceTarget ();
				}
			}else{
					
					StopAllCoroutines();
					Invoke("StartAITick",1.2f);
											
					Looks.StopAnimation();
				
				}

		}else if (targetCharacter == null) {
				StopAllCoroutines();
				
				Looks.StopAnimation();
			}


	
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
		
		switch(fightState){
		case FightState.Attack: FindTargetAndAttack(); break;
		case FightState.Fallback: FallBack(); break;
		case FightState.StandGround: StandGround(); break;
		case FightState.Flee: Flee(); break;
		}
	}
	public void CleanUpAfterFight(){
		StopAITick ();
		this.currentTarget = null;
		this.fightState = FightState.Idle;
		this.Looks.StartAnimation (AnimationNames.kWalk);
		this.transform.position = FightManager.FleeTarget.position;
	}
}
