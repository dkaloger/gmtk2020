using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class seed_manager : MonoBehaviour
{
    public Player p;
    public Text t;
    public int i;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      i = p._inventory["radishseeds"];
        t.text = i.ToString();
    }
}
