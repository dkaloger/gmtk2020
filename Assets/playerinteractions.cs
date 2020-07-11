using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerinteractions : MonoBehaviour
{
    //market
    public int my_money;
    public int watermelonprice;
    //  public int watermelonprice;
    //  public int watermelonprice;
    //  public int watermelonprice;
    public plant_player col;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plant" || collision.gameObject.tag == "plant") {
            if (collision.gameObject.GetComponent<plant_player>().currentgrowthstate_loc == 2 && Input.GetKeyDown("space"))
            {
                if (collision.gameObject.GetComponent<plant_player>().plant_type == 1)
                {
                    my_money += watermelonprice;
                }

                //  if (collision.gameObject.GetComponent<plant_player>().plant_type == 1)
                //   {
                //       my_money += watermelonprice;
                //   }
                //  if (collision.gameObject.GetComponent<plant_player>().plant_type == 1)
                //   {
                //        my_money += watermelonprice;
                //    }
                //  if (collision.gameObject.GetComponent<plant_player>().plant_type == 1)
                //  {
                //      my_money += watermelonprice;
                //    }


                Destroy(collision.gameObject);
            }
        }
    }
}
