using UnityEngine;
using System.Collections;

public class PayDebtInteraction : PlayerInteraction
{
    public override void Interact(float inputValue)
    {
        if (inputValue >= .5f)
        {
            var finances = _player.GetComponent<PlayerFinances>();
            finances.PayDebt();
        }
    }
}