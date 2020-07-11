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
        if (collision.gameObject.tag == "plant" || collision.gameObject.tag == "attackable-plant") {
            Debug.Log("l28");
            col = collision.gameObject.GetComponent<plant_player>();
            if (col.currentgrowthstate_loc == 2 )
            {
              //  if (Input.GetKeyDown(KeyCode.f))
                //{
                    Debug.Log("l31");
                    if (col.plant_type == 1)
                    {
                        my_money += watermelonprice;
                        Debug.Log("l35");
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
