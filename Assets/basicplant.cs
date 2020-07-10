using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicplant : MonoBehaviour
{
    //statts

    // public int growthstate1startingpoint;
    public int growthstate2startingpoint;

    public int growthstate3startingpoint;

    public int currentgrowthstate;

    public int currentgrowth;

    public int growth_coeficient;
    public Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentgrowth < growthstate3startingpoint)
        {
            currentgrowth++;
        }
     
        if (currentgrowth > growthstate2startingpoint && currentgrowth < growthstate3startingpoint)
        {
            currentgrowthstate = 2;
        }
        if (currentgrowth > growthstate3startingpoint)
        {
            currentgrowthstate = 3;
        }

        // change size 
        tr.localScale = new Vector3(currentgrowth / growth_coeficient, currentgrowth / growth_coeficient,currentgrowth / growth_coeficient);
    }
}