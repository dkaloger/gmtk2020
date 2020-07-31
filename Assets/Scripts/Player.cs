﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void InteractCallback(Transform playerInteractTarget);

[RequireComponent(typeof(Animator))] //Need this to animate player
[RequireComponent(typeof(Rigidbody))] //Need this for physics
[RequireComponent(typeof(CapsuleCollider))] //Need this for collision
public class Player : MonoBehaviour
{
	Rigidbody _rb;
	Animator _animator;

	[SerializeField]
	Transform rightHand = null;
	[SerializeField]
	Transform leftHand = null;
	public Transform ikTarget = null; //supposed to be the weed
	Transform _leftHandObj = null; //supposed to be the weed
	Transform _lookObj = null; //TODO: Implement.

	[Header("Player Control")]

	[SerializeField]
	protected float _speed = 1;

	protected bool _hasWheelbarrow = false; //TODO: toggle this state when the player grabs the wheelbarrow. //Possibly make a getter/setter so we can change the animator states to match this.

	Vector3 _facing = Vector3.forward;

	[SerializeField]
	protected float _timeBetweenTrips = 10f;
	private float _tripTimer = 0f;

	protected bool _canBeTripped = true;

	[Header("Player Inventory")]

	[SerializeField]
	protected GameObject[] _inventory;

	[Header("Player Reticle Settings")]

	[SerializeField]
	[Tooltip("This is that child object that is placed around the interaction target")]
	protected Transform _playerInteractionReticle;

	[SerializeField]
	float _playerInteractionRange = 60f;

	[SerializeField]
	float _playerInteractionReticleRotationSpeed = 1f;

	int _maxInteractionsToCheck = 30; // This determines how many colliders near the player can be checked to be made the "active" interaction item
	Transform _interactTarget; // Interaction trigger itself
	Collider[] _possibleInteractions; // Uses _maxInteractionsToCheck as array length

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_rb = GetComponent<Rigidbody>();
	}
	void Start() {
		_possibleInteractions = new Collider[_maxInteractionsToCheck];
	}

	void Update() {

		transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

		if (ikTarget)
		{
			ikTarget.position = rightHand.position;
			ikTarget.rotation = rightHand.rotation;
		}

		if(!_canBeTripped)
		{
			_tripTimer += Time.deltaTime;

			if (_tripTimer > _timeBetweenTrips)
			{
				_tripTimer = 0f;
				_canBeTripped = true;
			}
		}

		CheckForIneractiveTargets();
	}

	void CheckForIneractiveTargets()
	{
		Transform prevTarget = _interactTarget;
		_interactTarget = null;
		int numInteractions = Physics.OverlapCapsuleNonAlloc(transform.position + Vector3.up * 50, transform.position + Vector3.down * 50, _playerInteractionRange, _possibleInteractions, LayerMask.GetMask("Player Interaction Triggers"));
		Vector3 closestGoal = transform.position + transform.forward * (_playerInteractionRange / 2); //Using half the player interaction range as the target goal for what the player most wants to interact with.
		float distance = Mathf.Infinity;
		PlayerInteraction p;
		for (int i = 0; i < numInteractions; i++)
		{
			if (!_possibleInteractions[i].TryGetComponent<PlayerInteraction>(out p))
				Debug.LogError("An object the player is going to interact with needs to inherit from PlayerInteraction so universal properties can be shared.");
			
			if (_possibleInteractions[i].transform == transform || !_possibleInteractions[i].enabled || !p.active)
				continue; //We don't want to target ourself or any disabled colliders or this interaction has been disabled.

			//Get the closest interaction to the front of the player
			if (Vector3.Distance(_possibleInteractions[i].transform.position, closestGoal) < distance)
			{
				_interactTarget = _possibleInteractions[i].transform;
				distance = Vector3.Distance(_possibleInteractions[i].transform.position, closestGoal);
			}
		}

		if (_interactTarget != prevTarget && _interactTarget != null) // new target object
			_playerInteractionReticle.gameObject.SetActive(true);
				
		if (_interactTarget != null) // same target
		{
			_playerInteractionReticle.transform.Rotate(Vector3.up, _playerInteractionReticleRotationSpeed);
			_playerInteractionReticle.transform.position = _interactTarget.position;
		}
		else
			_playerInteractionReticle.gameObject.SetActive(false);
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

	public void OnFire(InputValue val) 
	{
		if (val.Get<float>() == 0)
			return;

		if (_interactTarget == null) //Player hasn't selected anything.
			return;

		//TODO: don't check for components, implement a callback or event of some kind. Move this logic where it should go: into the interaction classes!
		if (_interactTarget.TryGetComponent(out PlayerInteraction interact)) //weed a weed.
		{
			interact.Interact();
		}

		//What did we just interact with?
		switch (interact)
		{
			case null:
				Debug.LogError("PlayerInteraction returned null.");
				break;
			case WeedInteraction _:
				_animator.SetTrigger("Weed");
				break;
			case PlantInteraction _:
				_animator.SetTrigger("Plant");
				break;
			default:
				throw new System.NotImplementedException();
		}
	}

	//Called as an animation event from the pull plant animation when the player lets go during a toss of their right hand.
	public IEnumerator DiscardWeed()
	{
		Debug.Log("Discard Weed Method Called.");
		GameObject vineParent = null;

		if (ikTarget != null)
			vineParent = ikTarget.parent.parent.gameObject; //vineParent > vineMesh > vinePysics, Collision, & IK... this is why the: parent.parent
		else
			yield break;

		//ikTarget is always locked to the player's hand but we want that to be free, so we're unlocking it here.
		ikTarget = null;

		if (vineParent == null)
			yield break;

		//TODO: This should be inside the weed interaction function. Get it out of player.
		for (int i = 0; i < 40; i++) //i is 40 for 4 seconds updated 10 times per second:
		{
			SpringJoint[] joints = vineParent.GetComponentsInChildren<SpringJoint>();
			foreach (SpringJoint spring in joints)
			{
				spring.spring += 8f;
			}
			//weed spings should tighten significantly here.
			yield return new WaitForSeconds(0.1f);
		}

		vineParent.transform.position = new Vector3(9999f, 9999f, 9999f);
		Destroy(vineParent, .1f);
		Debug.Log("Destory Vine");

		yield break;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if(_canBeTripped && collision.transform.tag == "Vine")
		{
			//We've collided with a vine
			_animator.SetTrigger("Trip");
			_canBeTripped = false;
		}
	}

	//a callback for calculating IK
	public void OnAnimatorIK()
	{
		if (!_animator) // we want to transition these weights
			return;

		//IKStrengthCurve is a curve created in the pull weed animation and set through the animation controller. We can get it for a custom IK strength built for the animation.
		float ikStrength = _animator.GetFloat("IKStrengthCurve");

		// Set the right hand target position and rotation, if one has been assigned
		if (ikTarget != null)
		{
			_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikStrength);
			_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0); //was ikStrength
			_animator.SetIKPosition(AvatarIKGoal.RightHand, ikTarget.position);
			_animator.SetIKRotation(AvatarIKGoal.RightHand, ikTarget.rotation);

			_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikStrength);
			_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0); //was ikStrength
			_animator.SetIKPosition(AvatarIKGoal.LeftHand, ikTarget.position);
			_animator.SetIKRotation(AvatarIKGoal.LeftHand, ikTarget.rotation);

			_animator.SetLookAtWeight(1); // we want to look at the weed
			_animator.SetLookAtPosition(ikTarget.position); //_lookObj
		}
	}

	/// <summary>
	/// Draw Player Gizmos
	/// </summary>
	void OnDrawGizmosSelected()
	{
		return;

		//Draw a wire sphere representing the player's interaction range.
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, _playerInteractionRange);

		if(_interactTarget != null)
		{
			Gizmos.DrawSphere(_interactTarget.position, 2f);
		}
	}
}
