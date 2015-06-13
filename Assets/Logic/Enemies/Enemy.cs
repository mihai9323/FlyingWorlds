using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour {
	public FightState fightState;
	public EnemyLooks looks;
	public Skillset skillset;
	public Item weapon;
	[SerializeField] string fightAnimationName;

	private float attackRange{
		get{
			return weapon.Range;
		}
		
	}
	public AudioClip swordSlashSound;
	public AudioClip fireBallSound;
	public AudioClip bowSound;
	public AudioClip hitSound;

	public AI ai;
	public MonsterTypes monsterType;
	public float attackTime = 1.5f;
	public int Damage{
		get{
			switch(weapon.itemType){
			case Item.ItemType.Magic: return (int)(weapon.Damage * skillset.magic);
			case Item.ItemType.Melee: return (int)(weapon.Damage * skillset.melee);
			case Item.ItemType.Ranged: return (int)(weapon.Damage * skillset.archery);  
			}
			return 0;
		}
	}
	public int MaxHealth{
		get{
			return (int)Mathf.Max (2,skillset.health); 
		}
	}
	private int currentHealth;
	private Vector3 origScale;
	public bool dead;
	private Vector3 movement_target;
	private Transform movement_target_transform;
	[SerializeField]private float current_speed, movementSpeed;
	public bool Hit(int damage){
		currentHealth -= damage;
		if (hitSound != null)
			AudioSource.PlayClipAtPoint (hitSound, transform.position);
		if (currentHealth <= 1) {
			this.tag = "Dead";
			dead = true;
			FightManager.CheckWin();
			StopAllCoroutines();
			Destroy(this.gameObject,1f);
			return true;
		}
		return false;
	}

	public void MoveEnemyToPosition(Vector2 position, VOID_FUNCTION_ENEMY callback){
		movement_target = position;
		current_speed = movementSpeed;
		looks.StartAnimation (AnimationNames.kWalk);
		StopCoroutine("MoveToTransform");
		StopCoroutine("MoveToPosition");
		StartCoroutine("MoveToPosition",callback);
		
	}	
	public IEnumerator MoveToPosition(VOID_FUNCTION_ENEMY callback){
		this.audio.Play ();
		float remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		float initialDistance = Vector2.Distance(this.transform.position,movement_target);
		Vector2 initialPosition = transform.position;
		float ct = 0;
		while (remainingDistance>.2f) {
			FaceTarget(movement_target);
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
		this.audio.Stop ();
		looks.StopAnimation ();
		if (callback != null)
			callback (this);

	}
	private void Start(){
		float initZ = Mathf.Lerp(-10,0,Camera.main.WorldToScreenPoint(transform.position).y/Screen.height);
		transform.position = new Vector3(transform.position.x,transform.position.y,initZ);
	}
	public void MoveEnemyToTransform(Transform trans, VOID_FUNCTION_ENEMY callback){
		movement_target_transform = trans;
		current_speed = movementSpeed;
		looks.StartAnimation (AnimationNames.kWalk);
		StopCoroutine("MoveToTransform");
		StopCoroutine("MoveToPosition");
		StartCoroutine("MoveToTransform",callback);
		
	}	
	public IEnumerator MoveToTransform(VOID_FUNCTION_ENEMY callback){
		this.audio.Play ();
		movement_target = movement_target_transform.position;
		float remainingDistance = CustomSqrDistance (this.transform.position, movement_target);

		Vector2 initialPosition = transform.position;

		while (remainingDistance>attackRange*attackRange) {
			float prevX = transform.position.x;
			float initZ = transform.position.z;

			movement_target = movement_target_transform.position + movement_target_transform.right *attackRange *.5f;


			this.transform.position += (movement_target-transform.position).normalized * Time.deltaTime * movementSpeed;
			initZ = Mathf.Lerp(-10,0,Camera.main.WorldToScreenPoint(transform.position).y/Screen.height);
			transform.position = new Vector3(transform.position.x,transform.position.y,initZ);
			float curX = transform.position.x;


			yield return null;
			remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
			FaceDirection((int)Mathf.Sign(curX - prevX));
		}
		looks.StopAnimation ();
		this.audio.Stop ();
		if (callback != null)
			callback (this);

	}


	public void GenerateEnemy(bool isBoss = false){
		if (weapon == null || weapon.itemType == Item.ItemType.None || weapon.Damage == 0) {
			weapon = new Item (weapon.itemType, GameData.Progression);
		}
		looks.GenerateLooks (monsterType);
		looks.SetActiveWeapon (weapon);
		skillset.CreateSkillset (true);
		currentHealth = MaxHealth;
		origScale = transform.localScale;
		FaceDirection (-1);
		StartAITick ();
		dead = false;

	}
	public void FaceDirection(int direction){
		if(direction!=0){
			Vector3 lScale = origScale;
			lScale.x = Mathf.Abs (lScale.x) * Mathf.Sign (direction);
			transform.localScale = lScale;
		}
	}
	private void FaceTarget(Vector3 target){
		
		FaceDirection ((int)(target.x-transform.position.x));
		
	}
	private float CustomSqrDistance(Vector3 myPosition, Vector2 targetPosition){
		Vector2 auxPos = new Vector2 (myPosition.x, myPosition.y);
		return Vector2.SqrMagnitude(targetPosition - auxPos);
	}

	private IEnumerator AITick(){
		yield return new WaitForSeconds (.2f);
		while (!dead) {
			FindTarget();

			yield return new WaitForSeconds(.5f + Random.value*.5f);
		}
	}
	private Character currentTarget;

	private void FindTarget(){

		Character targetCharacter = null;
		GameObject gob= GameObject.FindGameObjectsWithTag("CHARACTER").OrderBy(go => Vector3.SqrMagnitude(go.transform.position- transform.position)- Mathf.Pow(go.GetComponent<Character>().primeTarget,2)).FirstOrDefault();
		if(gob!=null && gob.GetComponent<Character>()!=null) targetCharacter = gob.GetComponent<Character>();

		
			
		if (targetCharacter != null && currentTarget != targetCharacter) {
				
					currentTarget = targetCharacter;
					MoveEnemyToTransform (currentTarget.transform,
						                      delegate(Enemy e) {
						Attack (targetCharacter);
				
				});
				
		}else if (targetCharacter == currentTarget) {
			if (targetCharacter != null && CustomSqrDistance (this.transform.position, targetCharacter.transform.position)<attackRange*attackRange){
				Attack (targetCharacter);
			}else{
				if(targetCharacter!=null){
					MoveEnemyToTransform (currentTarget.transform,
					                      delegate(Enemy e) {
						Attack (targetCharacter);
						
					});
				}else{
					looks.StopAnimation ();
					StartAITick();
				}
			
			}
		} else if (targetCharacter == null) {
				
				looks.StopAnimation ();
				StartAITick();
			}



	}
	private void Attack (Character targetCharacter)
	{
		if (targetCharacter != null) {

			looks.StartAnimation (fightAnimationName);

			if(fightAnimationName == AnimationNames.kBowAttack){
				ShootProjectile(FightManager.arrow,targetCharacter);
				if(bowSound!=null)AudioSource.PlayClipAtPoint(bowSound,this.transform.position);
			}else if(fightAnimationName == AnimationNames.kMagicAttack){
				ShootProjectile(FightManager.fireBall,targetCharacter);
				if(fireBallSound!=null)AudioSource.PlayClipAtPoint(fireBallSound,this.transform.position);
			}else{

				targetCharacter.Hit (this.Damage,.5f);
				if(swordSlashSound!=null)AudioSource.PlayClipAtPoint(swordSlashSound,this.transform.position);
			}

		}
		StopAllCoroutines ();
		Invoke ("StartAITick", attackTime);
		
	}
	private void StartAITick(){
		if (this.gameObject.activeInHierarchy) {
			StopCoroutine ("AITick");
			StartCoroutine ("AITick");

		}
	}

	private void ShootProjectile(Projectile p, Character target){
		Projectile missile = Instantiate (p, transform.position, Quaternion.identity) as Projectile;
		missile.ShootCharacter (target.transform.position, this.Damage, null);
	}


}
