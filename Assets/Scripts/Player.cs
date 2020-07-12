using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public float _speed = 100;
	Vector3 _velocity;
	Vector3 _facing;
	public float _plantDistance = 1.5f;
	public health health;

	public Dictionary<string, int> _inventory;
	public static string[] ItemOrder = {
		"water", "radishseeds", "cornseeds", "watermelonseeds"
	};
	int _currentItemIndex;

	[SerializeField]
	protected Transform _model; //this is a reference to the child of the player and the model from Mixamo

	//reference to the animator used to control animations.
	Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	void Start() {
		_currentItemIndex = 0;
		_inventory = new Dictionary<string, int>();
		_inventory.Add("water", 5);
		_inventory.Add("radishseeds", 5);
		_inventory.Add("cornseeds", 5);
		_inventory.Add("watermelonseeds", 5);
		_facing = Vector3.forward;
	}

	public Vector3 GetPlantPosition()
    {
		return transform.position + _facing * _plantDistance;
    }

	void FixedUpdate() {
	
		var rigidbody = gameObject.GetComponent<Rigidbody>();
		if (health.stunned == false)
		{
			transform.Translate(_velocity * Time.fixedDeltaTime);
			if (_velocity != Vector3.zero)
			{
				_facing = _velocity.normalized;
				//_model = gameObject.GetComponentInChildren<MeshRenderer>(); // Set this in inspector
				if (_model)
                {
					_model.LookAt(transform.position + _facing);

				}
			}
		}

	}

	public void OnMove(InputValue val) {
		var move = val.Get<Vector2>();
		var rigidbody = gameObject.GetComponent<Rigidbody>();
		_velocity = new Vector3(move.x, 0, move.y) * _speed;

		_animator.SetFloat("Speed", Mathf.Clamp(_velocity.magnitude, 0f, 1f)); //tells the animator how fast we are going
		
	}

	int GetNumItem(string item) {
		if (!_inventory.ContainsKey(item)) {
			print("No such item "+item);
			return 0;
		}
		return _inventory[item];
	}

	void CycleItem(int delta) {
		_currentItemIndex = (_currentItemIndex + delta) % ItemOrder.Length;
		_currentItemIndex = (_currentItemIndex + ItemOrder.Length) % ItemOrder.Length;

		var item = ItemOrder[_currentItemIndex];
		var numItem = GetNumItem(item);
		print("Select "+item+", you have "+numItem);
	}

	public void OnNextItem(InputValue val) {
		if (val.Get<float>() > 0)
			CycleItem(1);
	}

	public void OnPrevItem(InputValue val) {
		if (val.Get<float>() > 0)
			CycleItem(-1);
	}

	public void OnScrollWheel(InputValue val) {
		var scroll = val.Get<Vector2>();
		print(scroll);
	}

	public void OnFire(InputValue val) {
		if (val.Get<float>() == 0)
			return;

		var item = ItemOrder[_currentItemIndex];
		var numItem = GetNumItem(item);

		if (numItem == 0) {
			print("Out of "+item);
		} else {
			--numItem;
			print("Used "+item+", you now have "+numItem);
			_inventory[item] = numItem;
			_animator.SetTrigger("Planting");
		}
	}
}
