using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerFinancesUI : MonoBehaviour
{
    private Text _cashText;
    private Text _debtText;

    void Start()
    {
        _cashText = transform.Find("Cash").GetComponent<Text>();
        _debtText = transform.Find("Debt").GetComponent<Text>();
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerFinances = player.GetComponent<PlayerFinances>();
        playerFinances.UpdateFinances += UpdateFinances;
    }

    void UpdateFinances(uint cash, uint debt)
    {
        _cashText.text = "$"+cash.ToString();
        _debtText.text = "You owe $"+debt.ToString();
    }
}