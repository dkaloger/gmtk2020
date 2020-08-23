using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateWatering : PlayerState
{
	public override void Enter()
	{
		base.Enter();

		_player.UpdateInteractTarget(Object.FindObjectOfType<WaterInteraction>().transform);
	}

	public override void Execute()
	{
		base.Execute();

		if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hitData, 1000))
			_player.interactTarget.position = hitData.point + Vector3.up * 0.5f;
	}

	public override void InteractWith(float inputValue)
	{
		base.InteractWith(inputValue);
	}

	public override void Tripped()
	{
		throw new System.NotImplementedException();
	}
}
