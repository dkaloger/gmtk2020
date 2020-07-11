using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class playerinteractions : MonoBehaviour
{
    //market
    public int my_money;
  //  public TextMeshProUGUI t;
  //  public TextMeshPro tmp;

    public int watermelonprice;

     public int parsnipprice;
      public int cornprice;
    //  public int watermelonprice;
    public plant_player col;
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        //planting



    }
    void OnCollisionEnter(Collision collision)
    {
        //planting

        //harvesting
        if (collision.gameObject.tag == "plant" || collision.gameObject.tag == "attackable-plant") {
            Debug.Log("l28");
            col = collision.gameObject.GetComponent<plant_player>();
            if (col.currentgrowthstate_loc == 2f )
            {
              //  if (Input.GetKeyDown(KeyCode.f))
                //{
             //       Debug.Log("l31");
                    if (col.plant_type == 1)
                    {
                        my_money += watermelonprice;
                   //     Debug.Log("l35");
                    }
                if (col.plant_type == 2)
                {
                    my_money += parsnipprice;
                //    Debug.Log("l35");
                }
                if (col.plant_type == 3)
                {
                    my_money += cornprice;
                    //    Debug.Log("l35");
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
               // }
              
            }
        }
    }
}
