using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seeds : MonoBehaviour
{
   public Vector3 spl;
  public  Player p;
    public spawn_seeds seedmaker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     //   transform.position = spl;
    }

    private void OnCollisionEnter(Collision collision)
    {

        p._inventory["radishseeds"] = p._inventory["radishseeds"] + 3;
        //   if (   p.it
        Destroy(this.gameObject);

        seedmaker.storedseeds -= 1;
    }
}
