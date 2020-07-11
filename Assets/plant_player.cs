using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plant_player : MonoBehaviour
{
    public watermelon watermelon;
    public sneaky_parnsnip parnsnip;
    //  watermelon watermelon;
    //  watermelon watermelon;
    //   watermelon watermelon;
    public int plant_type;
    // 1 watermelon,2 parnsnip,3corn,

    public int currentgrowth_loc;
    public int currentgrowthstate_loc;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (plant_type == 1)
        {
            currentgrowth_loc = watermelon.currentgrowth;
            currentgrowthstate_loc = watermelon.currentgrowthstate;

        }

        if (plant_type == 2)
        {
            currentgrowth_loc = parnsnip.currentgrowth;
            currentgrowthstate_loc = parnsnip.currentgrowthstate;
        }
    }
}
