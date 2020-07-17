using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))] //Need this to animate player
[RequireComponent(typeof(Rigidbody))] //Need this for physics
[RequireComponent(typeof(CapsuleCollider))] //Need this for collision
public class Player : MonoBehaviour
{
	public float _speed = 1;

	[SerializeField]
	protected Transform _model; //this is a reference to the child of the player and the model from Mixamo

	protected bool _hasWheelbarrow = false; //TODO: toggle this state when the player grabs the wheelbarrow. //Possibly make a getter/setter so we can change the animator states to match this.

	Vector3 _facing = Vector3.forward;

	[SerializeField]
	float _playerInteractionRange = 60f;

	//reference to the animator used to control animations.
	Animator _animator;

	int _maxInteractionsToCheck = 10; // This determines how many colliders near the player can be checked to be made the "active" interaction item

	Rigidbody _rb;

	Transform _interactTarget;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_rb = GetComponent<Rigidbody>();
	}
	void Start() {

	}

	void FixedUpdate() {

		transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

		Collider[] possibleInteractions = new Collider[_maxInteractionsToCheck];
		int numInteractions = Physics.OverlapCapsuleNonAlloc(transform.position + Vector3.up * 50, transform.position + Vector3.down * 50, _playerInteractionRange, possibleInteractions, LayerMask.GetMask("Player Interaction Triggers"));
		Debug.Log(numInteractions);

		_interactTarget = null; 
		Vector3 closestGoal = transform.position + transform.forward * (_playerInteractionRange / 2); //Using half the player interaction range as the target goal for what the player most wants to interact with.
		float distance = Mathf.Infinity;
		for (int i = 0; i < numInteractions; i++)
		{
			if (possibleInteractions[i].transform == transform)
				continue; //We don't avoid ourself or the target.

			//Get the closest interaction to the front of the player
			if (Vector3.Distance(possibleInteractions[i].transform.position, closestGoal) < distance)
			{
				_interactTarget = possibleInteractions[i].transform;
				distance = Vector3.Distance(possibleInteractions[i].transform.position, closestGoal);
			}
		}
	}

	public void OnMove(InputValue val) {
		Vector2 move = val.Get<Vector2>();
		
		if (_hasWheelbarrow) //move with wheelbarrow
		{
			_animator.SetFloat("Horizontal", move.x);
			_animator.SetFloat("Vertical", Mathf.Clamp(move.y, 0f, 1f)); //Clamp move.y because the player can't move backwards with the wheelbarrow
		}
		else //Move normal
		{
			if (move.sqrMagnitude > 0f) 
				_facing = new Vector3(move.x, 0, move.y); //only update facing if the player is inputing movement. This prevents the player from looking at 0,0,0 when no input is being given.
			_animator.SetFloat("Speed", Mathf.Clamp(move.magnitude, 0f, 1f)); //tells the animator how fast we are going
		}	
	}

	public void OnNextItem(InputValue val) {
		if (val.Get<float>() > 0)
		{

		}
	}

	public void OnPrevItem(InputValue val) {
		if (val.Get<float>() > 0)
		{

		}
	}

	public void OnScrollWheel(InputValue val) {
		var scroll = val.Get<Vector2>();
		print(scroll);
	}

	public void OnFire(InputValue val) {
		if (val.Get<float>() == 0)
			return;
				
	}

	/// <summary>
	/// Draw Player Gizmos
	/// </summary>
	void OnDrawGizmosSelected()
	{
		//Draw a wire sphere representing the player's interaction range.
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, _playerInteractionRange);

		if(_interactTarget != null)
		{
			Gizmos.DrawSphere(_interactTarget.position, 2f);
		}
	}
}
