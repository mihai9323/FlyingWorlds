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
	[SerializeField] float attackRange;
	public AI ai;

	public int Damage{
		get{
			switch(weapon.itemType){
			case Item.ItemType.Magic: return (int)(weapon.Damage * skillset.magic); break;
			case Item.ItemType.Melee: return (int)(weapon.Damage * skillset.melee); break;
			case Item.ItemType.Ranged: return (int)(weapon.Damage * skillset.archery); break;
			}
			return 0;
		}
	}
	public int MaxHealth{
		get{
			return (int)Mathf.Max (2,((skillset.magic + skillset.melee + skillset.archery) * GameData.TurnNumber)); 
		}
	}
	private int currentHealth;
	private Vector3 origScale;
	public bool dead;
	private Vector3 movement_target;
	private Transform movement_target_transform;
	[SerializeField]private float current_speed, movementSpeed;
	public void Hit(int damage){
		currentHealth -= damage;

		if (currentHealth <= 1) {
			dead = true;
			Destroy(this.gameObject);
		}
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
		looks.StopAnimation ();
		if (callback != null)
			callback (this);

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
		movement_target = movement_target_transform.position;
		float remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		float initialDistance = Vector2.Distance(this.transform.position,movement_target);
		Vector2 initialPosition = transform.position;
		float ct = 0;
		while (remainingDistance>attackRange) {
			FaceTarget();
			float initZ = transform.position.z;

			movement_target = movement_target_transform.position;




			this.transform.position += (-transform.position + movement_target).normalized * Time.deltaTime * movementSpeed;
			transform.position = new Vector3(transform.position.x,transform.position.y,initZ);
			yield return null;
			remainingDistance = CustomSqrDistance (this.transform.position, movement_target);
		}
		looks.StopAnimation ();
		if (callback != null)
			callback (this);

	}


	public void GenerateEnemy(){
		looks.GenerateLooks ();
		looks.SetActiveWeapon (weapon);
		skillset.CreateSkillset (true);
		currentHealth = MaxHealth;
		origScale = transform.localScale;
		FaceDirection (-1);
		StartAITick ();
		dead = false;

	}
	public void FaceDirection(int direction){
		Vector3 lScale = origScale;
		lScale.x = Mathf.Abs (lScale.x) * Mathf.Sign (direction);
		transform.localScale = lScale;
	}
	private void FaceTarget(){
		
		FaceDirection ((int)(movement_target.x-transform.position.x));
		
	}
	private float CustomSqrDistance(Vector3 myPosition, Vector2 targetPosition){
		Vector2 auxPos = new Vector2 (myPosition.x, myPosition.y);
		return Vector2.SqrMagnitude(targetPosition - auxPos);
	}

	private IEnumerator AITick(){
		yield return new WaitForSeconds (.2f);
		while (!dead) {
			FindTarget();

			yield return new WaitForSeconds(4.0f + Random.value*5);
		}
	}
	private Character currentTarget;

	private void FindTarget(){

		Character targetCharacter = null;
		GameObject gob= GameObject.FindGameObjectsWithTag("CHARACTER").OrderBy(go => Vector3.SqrMagnitude(go.transform.position- transform.position)).FirstOrDefault();
		if(gob!=null && gob.GetComponent<Character>()!=null) targetCharacter = gob.GetComponent<Character>();

		
			
			if (targetCharacter != null && targetCharacter != currentTarget) {
				currentTarget = targetCharacter;
				MoveEnemyToTransform (currentTarget.transform,
				                      delegate(Enemy e) {
					currentTarget.Hit (this.Damage);	
					StopCoroutine ("AITick");
					Invoke ("StartAITick", 1.2f);
					looks.StartAnimation (fightAnimationName);
					
				});
			} else if (targetCharacter == null) {
				StartAITick();
				looks.StopAnimation ();
			}



	}

	private void StartAITick(){
		if(this.gameObject.activeInHierarchy)StartCoroutine ("AITick");
	}




}
