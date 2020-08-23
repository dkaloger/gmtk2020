using ByTheTale.StateMachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// All the base/universal player behavior is going here. It will be overridden in subsequent child classes where it's different.
/// </summary>
public abstract class PlayerState : State
{
	protected Player _player;

	protected bool _canBeTripped = true;
	protected float _tripTimer = 0f; //how long since we've been tripped.

	public Transform prevTarget { get; private set; }

	public override void Initialize()
	{
		base.Initialize();

		_player = Object.FindObjectOfType<Player>();
	}

	public override void Execute()
	{
		base.Execute();

		if (!_canBeTripped)
		{
			_tripTimer += Time.deltaTime;

			if (_tripTimer > _player.timeBetweenTrips)
			{
				_tripTimer = 0f;
				_canBeTripped = true;
			}
		}

		UpdateReticle();
	}


	public override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

		if (_canBeTripped && collision.transform.tag == "Vine")
		{
			Tripped();
			_canBeTripped = false;
		}
	}

	public virtual void OnMove(Vector2 val)
	{
		//Vector2 move = val.Get<Vector2>();

		if (val.sqrMagnitude > 0f)
			_player.transform.rotation = Quaternion.LookRotation(new Vector3(val.x, 0, val.y), Vector3.up);

		_player.Anim.SetFloat("Speed", Mathf.Clamp(val.magnitude, 0f, 1f)); //tells the animator how fast we are going
	}

	/// <summary>
	/// Each state must handle how it responds to being tripped.
	/// </summary>
	public abstract void Tripped();

	public virtual void InteractWith(float inputValue)
	{
		//TODO: don't check for components, implement a callback or event of some kind. Move this logic where it should go: into the interaction classes!
		if (_player.interactTarget.TryGetComponent(out PlayerInteraction interact))
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
				_player.Anim.SetTrigger("Weed");
				break;
			case FarmingSpotInteraction _:
				_player.Anim.SetTrigger("Plant");
				break;
			case WaterInteraction _:
				_player.Anim.SetTrigger("Water");
				break;
			default:
				throw new System.NotImplementedException();
		}
	}

	public virtual void AltInteractWith(float input)
	{

	}

	/// <summary>
	/// Updates the red circle / reticle / selected / active thing indicator
	/// </summary>
	protected virtual void UpdateReticle()
	{
		//set the visibility of the player reticle
		if (_player.interactTarget != prevTarget && _player.interactTarget != null) // new target object
			_player.playerInteractionReticle.gameObject.SetActive(true);

		if (_player.interactTarget != null) // same target, animate the player reticle
		{
			_player.playerInteractionReticle.transform.Rotate(Vector3.up, _player.playerInteractionReticleRotationSpeed);
			_player.playerInteractionReticle.transform.position = _player.interactTarget.position;
		}
		else
			_player.playerInteractionReticle.gameObject.SetActive(false); //set the visibility of the player reticle
	}
}