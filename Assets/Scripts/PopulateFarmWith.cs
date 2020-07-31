using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateFarmWith : MonoBehaviour
{
    [Header("This script just copies whats listed here into the all the farming interaction spots.")]
    public bool pauseUpdatesToChildren = true;

    [SerializeField]
    protected PossiblePlants[] _plants;
	private void OnValidate()
	{
        if (pauseUpdatesToChildren)
            return;

        FarmingSpotInteraction[] spots = GetComponentsInChildren<FarmingSpotInteraction>();
        foreach (FarmingSpotInteraction spot in spots)
		{
            spot.possiblePlants = _plants;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
