using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plant is ready to be picked and harvested for profit.
/// </summary>
public class HarvestState : ByTheTale.StateMachine.State
{
    public override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

        //when we collide with the player we should be collected for all the money

        var finances = collision.collider.GetComponent<PlayerFinances>();
        if (finances)
        {
            var plant = GetMachine<Plant>();
            finances.DepositCash(plant.cashValue);
            GameObject.Destroy(plant.gameObject);
        }
    }

}
