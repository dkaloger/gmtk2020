using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
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
        if(pi.my_money > 100)
        {
            SceneManager.LoadScene("victory");
        }
        f.text = "coins:" + pi.my_money +"/100";
    }
}
