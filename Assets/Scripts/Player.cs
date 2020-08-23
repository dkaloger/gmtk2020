using ByTheTale.StateMachine;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void InteractCallback(Transform playerInteractTarget);

[RequireComponent(typeof(Animator))] //Need this to animate player
[RequireComponent(typeof(Rigidbody))] //Need this for physics
[RequireComponent(typeof(CapsuleCollider))] //Need this for collision
public class Player : MachineBehaviour //TODO: Switch the player to be a state machine. Set the player's inventory to be  a struct array that takes a prefab & a PlayerState and when we change the inventory item, we also get and change into the state assosiated with this item. This also means that someone trying to add a new item to the player will have to define it's state
{
	public Animator Anim { get; private set; }

	[SerializeField]
	Transform rightHand = null;
	public Transform ikTarget = null;

	[Header("Player Control")]

	[SerializeField]
	protected float _speed = 1;

	protected bool _hasWheelbarrow = false; //TODO: toggle this state when the player grabs the wheelbarrow. //Possibly make a getter/setter so we can change the animator states to match this.

	Vector3 _facing = Vector3.forward;

	public float timeBetweenTrips = 10f;

	[Header("Player Inventory")]

	public Transform inventoryRadialMenu;

	bool radialMenuOpen = false;
	Vector2 mouseRightClickPosition = Vector2.zero;
	Vector2 previousMousePos = Vector2.zero;

	public InventoryItemStatePair[] inventory;

	RMF_RadialMenu _menu;
	protected int _currentItem = 0;


	[Header("Player Reticle Settings")]

	[SerializeField]
	[Tooltip("This is that child object that is placed around the interaction target")]
	public Transform playerInteractionReticle;

	public float interactionRange = 10f;

	public float playerInteractionReticleRotationSpeed = 1f;

	int _maxInteractionsToCheck = 30; // This determines how many colliders near the player can be checked to be made the "active" interaction item
	internal Transform interactTarget { get; set; } // Interaction trigger itself
	public Collider[] possibleInteractions { get; private set; } // Uses _maxInteractionsToCheck as array length
	private Transform _prevTarget;

	/// <summary>
	/// we are overriding the currentState to require the use of a PlayerState so we can call specific fuctions in all states such as OnMove() and Interact()
	/// </summary>
	protected new PlayerState playerState;

	protected override State currentState { 
		get { 
			return playerState; 
		} 
		set { 
			playerState = (PlayerState)value; 
		}}

	private void Awake()
	{
		Anim = GetComponent<Animator>();
	}

	public override void Start() 
	{
		base.Start();

		_menu = inventoryRadialMenu.GetComponent<RMF_RadialMenu>();
		_menu.SetAllChildrenEnabled(false);
		possibleInteractions = new Collider[_maxInteractionsToCheck];
	}

	public override void AddStates()
	{
		AddState<PlayerStateMixedSeeds>();
		AddState<PlayerStateWatering>();
		AddState<PlayerStateFertilizer>();
		AddState<PlayerStateWheelbarrow>();

		SetInitialState<PlayerStateMixedSeeds>(); //Set the starting state.
	}

	public override void Update() 
	{
		base.Update();

		previousMousePos = Mouse.current.position.ReadValue();
		_menu.UpdateRadialMenuWithInput(previousMousePos - mouseRightClickPosition);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		Vector2 v = context.ReadValue<Vector2>();

		//Debug.Log(v);
		playerState.OnMove(v);
	}

	public void OnFire(InputAction.CallbackContext context)
	{
		if (interactTarget != null) //Player hasn't selected anything.
			playerState.InteractWith(context.ReadValue<float>());
	}

	public void OnAltFire(InputAction.CallbackContext context)
	{
		float input = context.ReadValue<float>();
		playerState.AltInteractWith(input); //we do need to do this to an InteractWithAlt() to handle this behavior based on the user's input

		mouseRightClickPosition = Mouse.current.position.ReadValue();
		radialMenuOpen = Convert.ToBoolean(input);

		if (radialMenuOpen)
			_menu.SetAllChildrenEnabled();
		else
			_menu.MakeSelection(Mouse.current.position.ReadValue() - previousMousePos);

	}

	public void SetStateToWater()
	{
		ChangeState<PlayerStateWatering>();
	}

	public void SetStateToFertilizer()
	{
		ChangeState<PlayerStateFertilizer>();
	}

	public void SetStateToMix()
	{
		ChangeState<PlayerStateMixedSeeds>();
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();
	}

	public void UpdateInteractTarget(Transform t)
	{
		//Check to see if the transform is something we can interact with:
		if (t.TryGetComponent<PlayerInteraction>(out _))
			interactTarget = t;
	}

	//Called as an animation event from the pull plant animation when the player lets go during a toss of their right hand.
	public IEnumerator DiscardWeed()
	{
		GameObject vineParent;

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
		//currentState.OnCollisionEnter(collision);
	}

	//a callback for calculating IK
	public void OnAnimatorIK()
	{
		if (!Anim) // we want to transition these weights
			return;

		//IKStrengthCurve is a curve created in the pull weed animation and set through the animation controller. We can get it for a custom IK strength built for the animation.
		float ikStrength = Anim.GetFloat("IKStrengthCurve");

		// Set the right hand target position and rotation, if one has been assigned
		if (ikTarget != null)
		{
			ikTarget.position = rightHand.position;
			ikTarget.rotation = rightHand.rotation;

			Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikStrength);
			Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0); //was ikStrength
			Anim.SetIKPosition(AvatarIKGoal.RightHand, ikTarget.position);
			Anim.SetIKRotation(AvatarIKGoal.RightHand, ikTarget.rotation);

			Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikStrength);
			Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0); //was ikStrength
			Anim.SetIKPosition(AvatarIKGoal.LeftHand, ikTarget.position);
			Anim.SetIKRotation(AvatarIKGoal.LeftHand, ikTarget.rotation);

			Anim.SetLookAtWeight(1); // we want to look at the weed
			Anim.SetLookAtPosition(ikTarget.position); //_lookObj
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
		Gizmos.DrawWireSphere(transform.position, interactionRange);

		if(interactTarget != null)
		{
			Gizmos.DrawSphere(interactTarget.position, 2f);
		}
	}
}
