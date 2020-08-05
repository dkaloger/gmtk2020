using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PossiblePlants
{
	public GameObject plant;
	[Tooltip("Spawn Chance = weight / all weights combine")]
	public float weight;
}

public class FarmingSpotInteraction : PlayerInteraction
{
    /// <summary>
    /// +1 on trigger enter, -1 on trigger exit
    /// </summary>
    int _count;

	/// <summary>
	/// Which plants are possible to spawn at this site.
	/// </summary>
	public PossiblePlants[] possiblePlants;

	protected float _totalSpawnWeight = 0f;

	// Update the total weight when the user modifies Inspector properties,
	// and on initialization at runtime.
	void OnValidate()
	{
		CalculateSpawnWeight();
	}

	public void Awake()
	{
		CalculateSpawnWeight();
	}

	public void CalculateSpawnWeight()
	{
		_totalSpawnWeight = 0f;
		foreach (PossiblePlants plant in possiblePlants)
			_totalSpawnWeight += plant.weight;
	}

	/// <summary>
	/// Spawn a random plant
	/// </summary>
	public override void Interact(float inputValue)
	{
		if (!active)
			return;

		active = false;
		// Generate a random position in the list.
		float pick = Random.value * _totalSpawnWeight;
		int chosenIndex = 0;
		float cumulativeWeight = possiblePlants[0].weight;

		// Step through the list until we've accumulated more weight than this.
		// The length check is for safety in case rounding errors accumulate.
		while (pick > cumulativeWeight && chosenIndex < possiblePlants.Length - 1)
		{
			chosenIndex++;
			cumulativeWeight += possiblePlants[chosenIndex].weight;
		}

		// Spawn the chosen item.
		Instantiate(possiblePlants[chosenIndex].plant.gameObject, transform.position, transform.rotation);
	}

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
