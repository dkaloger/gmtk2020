using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class moneytextmanager : MonoBehaviour
{
  public Text f;
   public playerinteractions pi;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        return; //added only so I can stop errors.
        f.text = "coins:" + pi.my_money +"/100";
    }
}
