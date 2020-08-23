using UnityEngine;
using UnityEditor;

public class HarvestInteraction : PlayerInteraction
{
	public Plant plant;
    public override void Interact(float inputValue)
    {
		if (inputValue >= .5f)
		{
			var playerFinances = _player.GetComponent<PlayerFinances>();
			playerFinances.DepositCash(plant.cashValue);
			GameObject.Destroy(plant.gameObject);
		}
    }
}