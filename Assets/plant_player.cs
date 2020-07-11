using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plant_player : MonoBehaviour
{
  public  watermelon watermelon;
    //  watermelon watermelon;
    //  watermelon watermelon;
    //   watermelon watermelon;
    public int plant_type; 
    // 1 watermelon,2 parnsnip,3corn,4 weeds

    public int currentgrowth_loc;
    public int currentgrowthstate_loc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentgrowth_loc = watermelon.currentgrowth;
          currentgrowthstate_loc = watermelon.currentgrowthstate;
    }
}
