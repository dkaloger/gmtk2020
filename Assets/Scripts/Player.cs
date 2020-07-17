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

	protected bool _hasWheelbarrow = false; //TODO: toggle this state when the player grabs the wheelbarrow.

	//reference to the animator used to control animations.
	Animator _animator;

	Rigidbody _rb;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_rb = GetComponent<Rigidbody>();
	}
	void Start() {

	}

	void FixedUpdate() {
		
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
			transform.rotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.y), Vector3.up);
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
}
