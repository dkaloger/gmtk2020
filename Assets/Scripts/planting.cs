using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planting : MonoBehaviour
{
    public Player p;
    public GameObject watermelon;
    public GameObject parsnip;
    public GameObject corn;
    public Transform player;
    public Vector3 player_pos;

 //public Vector3 q1_edge1;
    public Vector3 q1_edge2;

    public Vector3 q2_edge1;
    public Vector3 q2_edge2;

    public Vector3 q3_edge1;
    public Vector3 q3_edge2;
    public int my_q;
    public int rand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player_pos = player.position;
        my_q = 4;
       if (player_pos.x < 54.32999f && player_pos.z> 59.76308f)
       {
          my_q = 3;
      }

        if (player_pos.x < 53.4f && player_pos.z < 55.1f)
        {
            my_q = 2;
        }
        if (player_pos.x > 53.4f && player_pos.z > 58.8)
        {
            my_q = 1;
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            p._inventory["radishseeds"] = p._inventory["radishseeds"]-1;
            rand = Random.Range(1, 4);
            //for testing only removed
           // rand = 1;
          if (rand == 1)
            {
                Instantiate(watermelon, player_pos, new Quaternion(0f, 0f, 0f, 0f));
            }
          if (rand == 2)
         {
          Instantiate(parsnip, player_pos, new Quaternion(0f, 0f, 0f, 0f));
     }
            if (rand == 3)
            {
                Instantiate(corn, player_pos, new Quaternion(0f, 0f, 0f, 0f));
            }
       
        }

    }
}
