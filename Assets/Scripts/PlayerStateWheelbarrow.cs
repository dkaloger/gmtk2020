using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateWheelbarrow : PlayerState
{
	public override void Enter()
	{
		base.Enter();

		//we are entering the wheelbarrow state, update the animator to match player state
		_player.Anim.SetBool("HasWheelbarrow", true);
	}

	public override void Exit()
	{
		base.Exit();

		//we are exiting the wheelbarrow state, update the animator to match player state
		_player.Anim.SetBool("HasWheelbarrow", false);
	}

	public override void OnMove(Vector2 val)
	{
		//base.OnMove(val); //Don't call base.OnMove because we move differently when we have the wheelbarrow.
		//Vector2 move = val.Get<Vector2>();

		_player.Anim.SetFloat("Horizontal", val.x);
		_player.Anim.SetFloat("Vertical", Mathf.Clamp(val.y, 0f, 1f)); //Clamp move.y because the player can't move backwards with the wheelbarrow
	}

	public override void Tripped()
	{
		throw new System.NotImplementedException();
	}
}
