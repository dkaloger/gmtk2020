using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void InteractCallback(Transform playerInteractTarget);

[RequireComponent(typeof(Animator))] //Need this to animate player
[RequireComponent(typeof(Rigidbody))] //Need this for physics
[RequireComponent(typeof(CapsuleCollider))] //Need this for collision
public class Player : MonoBehaviour //TODO: Switch the player to be a state machine. Set the player's inventory to be  a struct array that takes a prefab & a PlayerState and when we change the inventory item, we also get and change into the state assosiated with this item. This also means that someone trying to add a new item to the player will have to define it's state
{
	Animator _animator;

	[SerializeField]
	Transform rightHand = null;
	public Transform ikTarget = null; //supposed to be the weed

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

	protected int _currentItem = 0;

	[Header("Player Reticle Settings")]

	[SerializeField]
	[Tooltip("This is that child object that is placed around the interaction target")]
	public Transform playerInteractionReticle;

	[SerializeField]
	float _playerInteractionRange = 60f;

	[SerializeField]
	float _playerInteractionReticleRotationSpeed = 1f;

	int _maxInteractionsToCheck = 30; // This determines how many colliders near the player can be checked to be made the "active" interaction item
	Transform _interactTarget; // Interaction trigger itself
	Collider[] _possibleInteractions; // Uses _maxInteractionsToCheck as array length
	private Transform _prevTarget;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	void Start() 
	{
		_possibleInteractions = new Collider[_maxInteractionsToCheck];
		UpdateInventoryState(); //we are going to start with an item equipped, make sure the player's state reflects it.
	}

	void Update() 
	{
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

		//if (current inventory item != wand)
		if (_currentItem == 0) //TODO: fix this so it isn't hardcoded.
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitData, 1000))
				_interactTarget.position = hitData.point + Vector3.up * 0.5f;
		}
		else //if (_currentItem == 1)
			CheckForIneractiveTargets();
		//else //move reticle to mouse position



		UpdateReticle();
	}
	void CheckForIneractiveTargets()
	{
		_prevTarget = _interactTarget; //Store previous target so we can see if we're going to get a new one or the same one.

		/* CHECK IF WATER IS EQUIPPED, TO CREATE A DYNAMIC INTERACT TARGET AT THE MOUSE POSITION */
		


		/* CHECK FOR EXISTING INTERACTIVE TARGETS IN THE WORLD ENVIORNMENT */
		
		//Physics overlap will get us a max of _possibleInteractions.Length colliders from the "Player Interaction Triggers" layer.
		int numInteractions = Physics.OverlapCapsuleNonAlloc(transform.position + Vector3.up * 50, transform.position + Vector3.down * 50, _playerInteractionRange, _possibleInteractions, LayerMask.GetMask("Player Interaction Triggers"));

		//Using half the player interaction range as the target goal for what the player most wants to interact with.
		Vector3 closestGoal = transform.position + transform.forward * (_playerInteractionRange / 2);

		_interactTarget = null; //Reset the interactTarget
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
	}

	/// <summary>
	/// Updates the red circle / reticle / selected / active thing indicator
	/// </summary>
	protected void UpdateReticle()
	{
		//set the visibility of the player reticle
		if (_interactTarget != _prevTarget && _interactTarget != null) // new target object
			playerInteractionReticle.gameObject.SetActive(true);

		if (_interactTarget != null) // same target, animate the player reticle
		{
			playerInteractionReticle.transform.Rotate(Vector3.up, _playerInteractionReticleRotationSpeed);
			playerInteractionReticle.transform.position = _interactTarget.position;
		}
		else
			playerInteractionReticle.gameObject.SetActive(false); //set the visibility of the player reticle
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
			_currentItem++;

			if (_currentItem >= _inventory.Length)
				_currentItem = 0;
		}

		UpdateInventoryState();
	}

	public void OnPrevItem(InputValue val) {
		if (val.Get<float>() > 0)
		{
			_currentItem--;

			if (_currentItem < 0)
				_currentItem = _inventory.Length - 1;
		}

		UpdateInventoryState();
	}

	//I'm thinking I can use this to determine the current inventory item and change based on this.
	public void UpdateInventoryState()
	{
		//TODO: this isn't the only inventory type, we need to implement a switch here.
		_interactTarget = FindObjectOfType<WaterInteraction>().transform;
	}

	public void OnScrollWheel(InputValue val) {
		var scroll = val.Get<Vector2>();
		print(scroll);
	}

	public void OnFire(InputValue val) 
	{
		Debug.Log("OnFire input value: " + val.Get<float>());

		if (_interactTarget != null) //Player hasn't selected anything.
		{
			InteractWith(val.Get<float>());
			return;
		}
	}

	protected void InteractWith(float inputValue)
	{
		//TODO: don't check for components, implement a callback or event of some kind. Move this logic where it should go: into the interaction classes!
		if (_interactTarget.TryGetComponent(out PlayerInteraction interact)) //weed a weed.
		{
			interact.Interact(inputValue);
		}

		if (inputValue <= 0)
			return; // we let go of the input, we shouldn't repeat the animations

		//What did we just interact with?
		switch (interact)
		{
			case null:
				Debug.LogWarning("PlayerInteraction returned null.");
				break;
			case WeedInteraction _:
				_animator.SetTrigger("Weed");
				break;
			case FarmingSpotInteraction _:
				_animator.SetTrigger("Plant");
				break;
			case WaterInteraction _:
				_animator.SetTrigger("Water");
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
