using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedInteraction : PlayerInteraction
{
    public Queue<Transform> vinesToPickBeforeDestorying = new Queue<Transform>();

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// This function destroys a single vine out of the weed, and if it's the last remaining vine: destroys gameobject
    /// </summary>
    /// <returns>The transform of a weed's vine.</returns>
    public Transform Weed()
	{
        Transform ret = null;

        if (vinesToPickBeforeDestorying.Count >= 1)
		{
            ret = vinesToPickBeforeDestorying.Dequeue();
		}

        Transform originGO = ret.parent.parent; //.DetachChildren();
        originGO.GetComponent<Vine>().reverseAnimation = true;
        //Destroy(originGO.gameObject, 10f);


        //TODO: change parent of transform to player hand.

        //we want to destory self if we're the last thing existing. but we should only have a few vines for show and nothing the player can interact with... 
        //so it will be safe to destroy without disrupting the fancy physics animations
        if (vinesToPickBeforeDestorying.Count <= 0)
        {
            //OnTriggerExit doesn't fire if the collider is disabled or destroyed, so I'm taking the hacky way and moving it into the void before destroying it a split second later).
            //transform.parent.position = new Vector3(9999f, 9999f, 9999f);
            //Destroy(transform.parent.gameObject, .1f);
        }
        return ret;
	}


    // Update is called once per frame
    void Update()
    {
        
    }
}