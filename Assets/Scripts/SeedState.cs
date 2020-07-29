using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the starting state and just waits to be watered before moving to the next state
/// </summary>
public class SeedState : ByTheTale.StateMachine.State
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Goes directly to the growing state.
    /// </summary>
    public void Water()
	{
        machine.ChangeState<GrowingState>();
	}
}
