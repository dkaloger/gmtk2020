using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plant is growing, it could grow into a pick-able plant or a monster. Fertilizer will make it grow faster, but also more likely to be a monster
/// </summary>
public class GrowingState : ByTheTale.StateMachine.State
{
	public Plant plant { get { return (Plant)machine; } }

	public override void Enter()
	{
		base.Enter();

		plant.transform.localScale = Vector3.one * plant.startingSize;
	}

	public override void Execute()
	{
		base.Execute();

		plant.transform.localScale = Vector3.MoveTowards(plant.transform.localScale, Vector3.one, 1 / plant.timeTillFullyGrown * Time.deltaTime);
		
		if(plant.transform.localScale.x >= 1) //fully grown
		{
			if (Random.value <= plant.corruption)
				plant.ChangeState<HarvestState>();
			else
				plant.ChangeState<MonsterState>();
		}
	}
}