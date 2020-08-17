using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInteraction : PlayerInteraction
{
	Cloud _cloud;

    // There's only going to be one water interaction in the game, because it doesn't technically do any watering it's just set to the mouse position and then the cloud's target.
	public override void Interact(float inputValue)
	{
		if (inputValue == 0)
			_cloud.ResetTargetToPlayer();
		else
			_cloud.GoRainAtMousePosition();
	}

	// Start is called before the first frame update
	void Start()
    {
		if (!_cloud)
			_cloud = FindObjectOfType<Cloud>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDrawGizmos()
	{
		
	}
}
