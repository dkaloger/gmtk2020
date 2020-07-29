using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingSpotInteraction : PlayerInteraction
{
    /// <summary>
    /// +1 on trigger enter, -1 on trigger exit
    /// </summary>
    int _count;

	public void OnTriggerEnter(Collider other)
	{
        _count++;
        active = false;
	}

	public void OnTriggerExit(Collider other)
	{
        _count--;

        if (_count <= 0)
            active = true;
	}
}
