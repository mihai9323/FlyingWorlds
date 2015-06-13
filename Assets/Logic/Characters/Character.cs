using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character : MonoBehaviour {


	public FightState fightState;

	public string Name;
	public TraitManager.TraitTypes[] Traits;
	public Skillset Skills;
	public CharacterLooks Looks;
	public Portrait CharacterPortrait;


	public float movementSpeed{
		get{
			return _movementSpeed + ((this.weaponItem!=null && this.weaponItem.itemType == Item.ItemType.Melee)? 1.4f:
			                         (this.weaponItem!=null && this.weaponItem.itemType == Item.ItemType.None)? 1.4f:
			                         (this.weaponItem==null)? 1.4f:0
			                         );
		}
	}
	[SerializeField] float _movementSpeed = 1.4f;
	[SerializeField] float fleeSpeed = 1.5f;
	public bool inFightingParty;
	public bool fled;

	public AudioClip swordSlashSound;
	public AudioClip fireBallSound;
	public AudioClip bowSound;
	public AudioClip hitSound;
	
	public Trait trait0{
		get{
			return TraitManager.GetTrait(Traits[0]);
		}
	}
	public Trait trait1{
		get{
			return TraitManager.GetTrait(Traits[1]);
		}
	}
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
	public int damageDealtInLastBattle;
	public bool foughtInLastBattle;
	Enemy lockedFarTarget;
	public Enemy currentTarget;
	public int Level{
		get{
			return Mathf.Max((int)((this.Skills.archery + this.Skills.magic + this.Skills.melee)/2.5f),1);
		}
	}/// <summary>
	/// Gets the orders the character follows 
	/// 0 - Flee
	/// 1 - Stand Ground
	/// 2 - Retreat and Stand ground
	/// 3 - All
	/// </summary>
	/// <value>The orders.</value>
	public int Orders{
		get{
			if(trait0.debuffs.Contains(BuffsAndDebuffs.BuffType.StandGround) || trait1.debuffs.Contains(BuffsAndDebuffs.BuffType.StandGround)){
				float morale = CalculateMoral();
				if(morale<.2f) return 0;
				if(morale<.3f) return 1;
				if(morale<.4f) return 2;
			}
			return 3;
		}
	}
	public Dictionary<LabelManager.LabelType,Label> labels;
	public bool Ready;
	public float attackTime  = 1.3f;
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
			
			return (int)(50+ Level * 10);
		}
	}
	public int armor, damage;
	public float primeTarget;
	public int Armor {
		get{
			return (int)((ArmorItem.Defence+WeaponItem.Defence) *  (BuffInfluence(new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.MoreDefenseSelf,BuffsAndDebuffs.BuffType.LessDefenseSelf},
															new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.MoreDefenseSelf,BuffsAndDebuffs.BuffType.LessDefenseSelf},
															1,
															CalculateMoral())
			             					  +CharacterManager.partyDefence
			                                  + (float)Level/10)
			);
		}
	}

	public float PrimeTarget{
		get{
			return BuffInfluence(new BuffsAndDebuffs.BuffType[1]{BuffsAndDebuffs.BuffType.PrimeTargetSelf},null,0,CalculateMoral());
		}
	}
	public int Damage{
		get{
			if(WeaponItem.itemType == Item.ItemType.Magic){
						return (int)((Level + Skills.magic+(
							BuffInfluence(new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.MoreMagicDamageSelf,BuffsAndDebuffs.BuffType.MoreDamgeSelf},
										  new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.LessMagicDamageSelf,BuffsAndDebuffs.BuffType.LessDamageSelf},
										  1,
										  CalculateMoral())
						   +CharacterManager.partyDamageBonus)) * WeaponItem.Damage );			
			}else if(WeaponItem.itemType == Item.ItemType.Melee){
						return (int)((Level + Skills.melee+(
							BuffInfluence(new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.MoreMeleeDamageSelf,BuffsAndDebuffs.BuffType.MoreDamgeSelf},
							new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.LessMeleeDamageSelf,BuffsAndDebuffs.BuffType.LessDamageSelf},
							1,
							CalculateMoral())
						   +CharacterManager.partyDamageBonus)) * WeaponItem.Damage );			
			}else if(WeaponItem.itemType == Item.ItemType.Ranged){
						return (int)((Level + Skills.archery+(
						BuffInfluence(new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.MoreBowDamageSelf,BuffsAndDebuffs.BuffType.MoreDamgeSelf},
						new BuffsAndDebuffs.BuffType[2]{BuffsAndDebuffs.BuffType.LessBowDamageSelf,BuffsAndDebuffs.BuffType.LessDamageSelf},
						1,
						CalculateMoral())
					   +CharacterManager.partyDamageBonus)) * WeaponItem.Damage );			
			}else return (int)(Level + Skills.melee );			
		}
	}
	public int GearValue{
		get{
			int val = 0;
			if (WeaponItem != null && WeaponItem.itemType != Item.ItemType.None) {
				val += WeaponItem.Value;
			}
			if (ArmorItem != null && ArmorItem.itemType != Item.ItemType.None) {
				val += ArmorItem.Value;
			}
			return val;
		}
	}
	public int Moral{
		get{
			return (int)Mathf.Clamp (CalculateMoral()*100,0,100);
		}
	}

	private Vector2 movement_target;
	private float current_speed;
	private Vector3 origScale;

	private Transform movement_target_transform;
	private void Awake(){
		labels = new Dictionary<LabelManager.LabelType, Label> ();
		origScale = transform.localScale;
		PlaceOnLayer ();
	}

	public void SetStaticStats(){
		damage = Damage;
		armor = Armor;
		primeTarget = PrimeTarget;
	}

	public float CalculateMoral(){
		int moral = 5;
		foreach (Label l in labels.Values) {
			if(TraitManager.GetTrait(this.Traits[0]).influencedBy.Contains(l.labelType) || TraitManager.GetTrait(this.Traits[1]).influencedBy.Contains(l.labelType)){
				moral += l.moraleChange * Mathf.Clamp(GameData.Progression - l.receivedTurn+1,1,5);
			}
		}
		if (this == CharacterManager.hasWorstGear) {
			moral += CharacterManager.worstGearMoraleBonus;
		}
		if (this == CharacterManager.hasBestGear) {
			moral += CharacterManager.bestGearMoraleBonus;
		}
		moral = Mathf.Clamp (moral + CharacterManager.partyMoral, 0, 10);
		return (float)moral / 10;
	}
	public float CalculatePlainMoral(){
		int moral = 5;
		foreach (Label l in labels.Values) {
			if(TraitManager.GetTrait(this.Traits[0]).influencedBy.Contains(l.labelType) || TraitManager.GetTrait(this.Traits[1]).influencedBy.Contains(l.labelType)){
				moral += l.moraleChange * Mathf.Clamp(GameData.Progression - l.receivedTurn+1,1,3);
			}
		}
		moral = Mathf.Clamp (moral, 0, 10);
		return (float)moral / 10;
	}
	public float BuffInfluence(BuffsAndDebuffs.BuffType[] buffs, BuffsAndDebuffs.BuffType[] debuffs, byte mode,float morale){
		float v = mode;
		if (buffs != null) {
			foreach (BuffsAndDebuffs.BuffType buff in buffs) {
				if (trait0.buffs.Contains (buff) || trait1.buffs.Contains (buff)) {
					if (mode == 0)
						v += (BuffsAndDebuffs.buffDefinitions [buff].maxAdditiveValue * Mathf.Clamp ((morale - 0.5f) * 2, 0, 1));
					if (mode == 1)
						v += (BuffsAndDebuffs.buffDefinitions [buff].maxEffectPercentValue * Mathf.Clamp ((morale - 0.5f) * 2, 0, 1));
				}
			}
		}
		if (debuffs != null) {
			foreach (BuffsAndDebuffs.BuffType debuff in debuffs) {
				if (trait0.debuffs.Contains (debuff) || trait1.debuffs.Contains (debuff)) {
					if (mode == 0)
						v -= (BuffsAndDebuffs.buffDefinitions [debuff].maxAdditiveValue * Mathf.Clamp ((1 - morale * 2), 0, 1));
					if (mode == 1)
						v -= (BuffsAndDebuffs.buffDefinitions [debuff].maxEffectPercentValue * Mathf.Clamp ((1 - morale * 2), 0, 1));
				}
			}
		}
		return v;
	}
	

	
	public void CreateCharacter(){
		if (CharacterManager.generateCharacters) {
			Traits = TraitManager.GetRandomTraitsTypes ();
			Skills.CreateOneSkillset ();
			Name = CharacterManager.GenerateCharacterName (trait0,trait1);
			Looks.GenerateLooks ();
		} 
		Health = MaxHealth;
		Ready = true;

	}
	
	private string iThink(){
		string message="";
		foreach (TraitManager.TraitTypes t in Traits) {
			Trait trait = TraitManager.GetTrait(t);
			foreach(LabelManager.LabelType lt in trait.influencedBy){
				if(labels.ContainsKey(lt)){
					message+= labels[lt].effectString(t)+"\n";
				}else{
					string name,location,color,monster;
					LabelManager.GetLabel(lt).isActive(this,out name,out location,out color,out monster);

					if((LabelManager.GetLabel(lt).hasNotAccomplished) && labels.ContainsKey(LabelManager.GetLabel(lt).prerequisite))message += LabelManager.GetLabel(lt).notAccomplishedString(name,location,color,monster)+"\n";
				}
			}
			/*
			foreach(Label l in labels.Values){

				if(trait.influencedBy.Contains(l.labelType)){
					message+= l.effectString(t)+"\n";
				}
			}
			*/
		}
		return message;
	}

	public void Hit(int damage,float time=0){
		if (hitSound != null)
			AudioSource.PlayClipAtPoint (hitSound, transform.position);
		Debug.Log ("Get hit");
		StartCoroutine (BeHitDelayed (time,damage));

	}
	private IEnumerator BeHitDelayed(float time,int damage){
		if(time>0)yield return new WaitForSeconds (time);
		Debug.Log (damage);
		Health -= Mathf.Max (Mathf.Max (0,damage-armor),1);
		Debug.Log ("applied dmg:" + Mathf.Max (Mathf.Max (0, damage - armor), 1));
		if (Health <=Mathf.Max(1, MaxHealth * (.2f- CharacterManager.partyFlee*.2f))) {
			fightState = FightState.Flee;
			
			this.tag = "Dead";
		}
	}

	public void MoveCharacterToPosition(Vector2 position, VOID_FUNCTION_CHARACTER callback){
		movement_target = position;
		current_speed =  (fightState == FightState.AttackClose || fightState == FightState.Idle || fightState == FightState.AttackFar)? movementSpeed : fleeSpeed;
		Looks.StartAnimation (AnimationNames.kWalk);
		StopCoroutine("MoveToPosition");
		StopCoroutine ("MoveToTransform");
		StartCoroutine("MoveToPosition",callback);
	}


	public IEnumerator MoveToPosition(VOID_FUNCTION_CHARACTER callback){
		this.audio.Play ();
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
			PlaceOnLayer();
			yield return null;
			remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		}
		Looks.StopAnimation ();
		this.audio.Stop ();
		if (callback != null)
			callback (this);

	}
	public void PlaceOnLayer ()
	{
		float z = Mathf.Lerp (-10, 0, Camera.main.WorldToScreenPoint (transform.position).y / Screen.height);
		transform.position = new Vector3 (transform.position.x, transform.position.y, z);
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
				this.foughtInLastBattle = true;
			}else Debug.LogWarning("Allready in party");
		} else
			HubManager.notification.ShowNotification("Your fighting party is to large \n Kick someone out before inviting a new member!","Sure!");
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




	public void MoveCharacterToTransform(Transform trans, VOID_FUNCTION_CHARACTER callback){
		movement_target_transform = trans;
		current_speed = movementSpeed;
		Looks.StartAnimation (AnimationNames.kWalk);
		StopCoroutine("MoveToTransform");
		StopCoroutine("MoveToPosition");
		StartCoroutine("MoveToTransform",callback);
		
	}	
	public IEnumerator MoveToTransform(VOID_FUNCTION_CHARACTER callback){
		this.audio.Play ();
		movement_target = movement_target_transform.position;
		float remainingDistance = CustomSqrDistance (this.transform.position, movement_target);


		float ct = 0;
		while (remainingDistance>weaponItem.Range*weaponItem.Range && movement_target_transform!=null) {
			FaceTarget();
			float initZ = transform.position.z;
			
			movement_target = movement_target_transform.position;
			
			
			
			
			this.transform.position += (-transform.position + (Vector3)movement_target).normalized * Time.deltaTime * movementSpeed;
			PlaceOnLayer();
			yield return null;
			remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		}
		this.audio.Stop ();
		Looks.StopAnimation ();
		if (callback != null)
			callback (this);
		
	}



	private void FindCloseTargetAndAttack(){
		var target = FindClosestTarget ();

			
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

	private void FindFarTargetAndAttack(){

		if (lockedFarTarget == null || lockedFarTarget.dead) {
			lockedFarTarget = FindFurthestTarget ();
		}

		if (lockedFarTarget == null) {
			StopFight ();
		} else {
			if (IsNewTarget (lockedFarTarget)) {
				MoveAndAttack (lockedFarTarget);
			} else if (IsSameTarget (lockedFarTarget)) {
				if (IsInWeaponRange (lockedFarTarget)) {
					Attack (lockedFarTarget);						
				} else {
					MoveAndAttack (lockedFarTarget);
				}
			}
		}
	}

	private void StandGround(){ 
		var target = FindClosestTarget ();
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
	private void Flee(){
		fled = true;
		MoveCharacterToPosition (FightManager.FleeTarget.position, delegate(Character c) {
			StopAITick();
			FightManager.CheckLost();
		});
	}

	private void MoveAndAttack (Enemy targetEnemy)
	{
		currentTarget = targetEnemy;
		MoveCharacterToTransform (targetEnemy.transform, delegate (Character c) {
			Attack (targetEnemy);
		});
	}
	private void Attack (Enemy targetEnemy)
	{
		if (targetEnemy != null) {
		
			Looks.StartAnimation (weaponItem.fightAnimation);
			FaceDirection((int)(targetEnemy.transform.position.x - this.transform.position.x));
			if(weaponItem.fightAnimation == AnimationNames.kBowAttack){
				ShootProjectile(FightManager.arrow,targetEnemy);
				damageDealtInLastBattle += this.damage;
				Skills.archery+=.05f;
				if(bowSound!=null)AudioSource.PlayClipAtPoint(bowSound,this.transform.position);

			}else if(weaponItem.fightAnimation == AnimationNames.kMagicAttack){
				ShootProjectile(FightManager.fireBall,targetEnemy);
				damageDealtInLastBattle += this.damage;
				Skills.magic+=.1f;
				if(fireBallSound!=null)AudioSource.PlayClipAtPoint(fireBallSound,this.transform.position);

			}else{
				if(targetEnemy.Hit (this.damage)){
					if(this.weaponItem.monstersKilled.ContainsKey(targetEnemy.monsterType)){
						this.weaponItem.monstersKilled[targetEnemy.monsterType]++;
					}else this.weaponItem.monstersKilled.Add(targetEnemy.monsterType,1);
				}
				if(swordSlashSound!=null)AudioSource.PlayClipAtPoint(swordSlashSound,this.transform.position);
				damageDealtInLastBattle += this.damage;
				Skills.melee+=.3f;

			}

		}
		StopAllCoroutines ();
		Invoke ("StartAITick", attackTime);

	}
	private void ShootProjectile(Projectile p, Enemy target){
		Projectile missile = Instantiate (p, transform.position, Quaternion.identity) as Projectile;
		missile.ShootMonster (target.transform.position, this.damage, delegate(Enemy e) {
			if(e!=null && e.dead){
				if(this.weaponItem.monstersKilled.ContainsKey(e.monsterType)){
					this.weaponItem.monstersKilled[e.monsterType]++;
				}else this.weaponItem.monstersKilled.Add(e.monsterType,1);
			}
		});
	}
	private void StopFight ()
	{
		StopAITick ();
		Looks.StopAnimation ();
	}
	private Enemy FindClosestTarget ()
	{
		Enemy targetCharacter = null;
		GameObject gob = GameObject.FindGameObjectsWithTag ("ENEMY").OrderBy (go => Vector3.SqrMagnitude (go.transform.position - transform.position)).FirstOrDefault ();
		if (gob != null && gob.GetComponent<Enemy> () != null)
			targetCharacter = gob.GetComponent<Enemy> ();
		return targetCharacter;
	}
	private Enemy FindFurthestTarget ()
	{
		Enemy targetCharacter = null;
		GameObject gob = GameObject.FindGameObjectsWithTag ("ENEMY").OrderBy (go => Vector3.SqrMagnitude (go.transform.position - transform.position)).LastOrDefault ();
		if (gob != null && gob.GetComponent<Enemy> () != null)
			targetCharacter = gob.GetComponent<Enemy> ();
		return targetCharacter;
	}
	private bool IsNewTarget (Enemy targetEnemy)
	{
		return targetEnemy != null && currentTarget != targetEnemy;
	}
	private bool IsSameTarget (Enemy targetEnemy)
	{
		return targetEnemy == currentTarget;
	}
		
	private bool IsInWeaponRange (Enemy target, float percent = 1.0f)
	{
		return CustomSqrDistance (this.transform.position, target.transform.position) < weaponItem.Range * weaponItem.Range * percent;
	}

	private void FallBack(){
		MoveCharacterToPosition (FightManager.RetreatTarget.position, delegate(Character c) {
			fightState = FightState.StandGround;
		});
		fightState = FightState.Fallback;
	}
	private IEnumerator AITick(){
		while (!fled) {
			Tick ();
			yield return new WaitForSeconds(.05f + Random.value*.05f);
		}
	}
	public void Tick(){
		if (!fled) {
			switch (fightState) {
			case FightState.AttackClose:
				FindCloseTargetAndAttack ();
				break;
			case FightState.AttackFar:
				FindFarTargetAndAttack();
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
		//todo calculate labels here
		this.StopAllCoroutines ();
		this.currentTarget = null;
		this.fightState = FightState.Idle;
		this.Looks.StopAnimation ();
		this.transform.position = HubManager.road.RoadWorldSpace.position;
		this.fled = false;
		RemoveFromParty ();
		damageDealtInLastBattle = 0;
		this.Looks.SetLight (Color.white);
	}
	private void OnMouseUp(){

		if (GameData.currentScene == GameScenes.Hub && HubManager.interactable && HubManager.noPanelOpened) {
			Debug.Log ("clicked");
			HubManager.ShowCharacters(this);
		}
	}
}
