using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMixedSeeds : PlayerState
{
	public override void Execute()
	{
		base.Execute();

		/* CHECK FOR EXISTING INTERACTIVE TARGETS IN THE WORLD ENVIORNMENT */

		//prevTarget = _player.interactTarget; //Store previous target so we can see if we're going to get a new one or the same one.

		//Physics overlap will get us a max of _possibleInteractions.Length colliders from the "Player Interaction Triggers" layer.
		int numInteractions = Physics.OverlapCapsuleNonAlloc(_player.transform.position + Vector3.up * 50, _player.transform.position + Vector3.down * 50, _player.interactionRange, _player.possibleInteractions, LayerMask.GetMask("Player Interaction Triggers"));

		//Using half the player interaction range as the target goal for what the player most wants to interact with.
		Vector3 closestGoal = _player.transform.position + _player.transform.forward * (_player.interactionRange / 2);

		_player.interactTarget = null; //Reset the interactTarget
		float distance = Mathf.Infinity;
		for (int i = 0; i < numInteractions; i++)
		{
			if (!_player.possibleInteractions[i].TryGetComponent<PlayerInteraction>(out PlayerInteraction p))
				Debug.LogError("An object the player is going to interact with needs to inherit from PlayerInteraction so universal properties can be shared.");

			if (_player.possibleInteractions[i].transform == _player.transform || !_player.possibleInteractions[i].enabled || !p.active)
				continue; //We don't want to target ourself or any disabled colliders or this interaction has been disabled.

			//Get the closest interaction to the front of the player
			if (Vector3.Distance(_player.possibleInteractions[i].transform.position, closestGoal) < distance)
			{
				_player.interactTarget = _player.possibleInteractions[i].transform;
				distance = Vector3.Distance(_player.possibleInteractions[i].transform.position, closestGoal);
			}
		}
	}

	public override void InteractWith(float inputValue)
	{
		base.InteractWith(inputValue);

		//Interact With the farming spot that was selected.
	}

	public override void Tripped()
	{
		throw new System.NotImplementedException();
	}
}
