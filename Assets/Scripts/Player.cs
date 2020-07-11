using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public float _speed = 100;
	Vector3 _velocity;

	void Start() {
	}

	void FixedUpdate() {
		var rigidbody = gameObject.GetComponent<Rigidbody>();
		transform.Translate(_velocity * Time.fixedDeltaTime);
	}

	public void OnMove(InputValue val) {
		var move = val.Get<Vector2>();
		var rigidbody = gameObject.GetComponent<Rigidbody>();
		_velocity = new Vector3(move.x, 0, move.y) * _speed;
	}

	public void OnLook(InputValue val) {
	}

	public void OnFire(InputValue val) {
	}
}
