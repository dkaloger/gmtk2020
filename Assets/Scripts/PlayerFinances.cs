using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Assertions;

public class PlayerFinances : MonoBehaviour
{
    uint _cash = 0;
    uint _debt = 0;

    public uint InitialDebt = 10000;

    public delegate void UpdateFinancesDelegate(uint cash, uint debt);
    public event UpdateFinancesDelegate UpdateFinances;

    // Use this for initialization
    void Start()
    {
        _debt = InitialDebt;
        UpdateFinances?.Invoke(_cash, _debt);
    }

    public void DepositCash(uint cash)
    {
        _cash += cash;
        UpdateFinances?.Invoke(_cash, _debt);
    }

    public void AddDebt(uint debt)
    {
        _debt += debt;
        UpdateFinances?.Invoke(_cash, _debt);
    }

    public void PayDebt()
    {
        var payment = Math.Min(_cash, _debt);
        _cash -= payment;
        _debt -= payment;
        UpdateFinances?.Invoke(_cash, _debt);
    }
}