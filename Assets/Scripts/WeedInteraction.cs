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

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This function destroys a single vine out of the weed, and if it's the last remaining vine: destroys gameobject
    /// </summary>
    /// <returns>The transform of a weed's vine.</returns>
    public override void Interact(float inputValue)
	{
        Debug.Log("Weed Interact Fired");
        if (vinesToPickBeforeDestorying.Count >= 1)
        {
            Transform temp = _player.ikTarget = vinesToPickBeforeDestorying.Dequeue();
            temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            Transform origin = temp.parent.parent;
            origin.GetComponent<Vine>().reverseAnimation = true;

            Debug.Log("Player should have their interact target set now.");
        }
        
        //we want to destory self if we're the last thing existing. but we should only have a few vines for show and nothing the player can interact with... 
        //so it will be safe to destroy without disrupting the fancy physics animations
        if (vinesToPickBeforeDestorying.Count <= 0)
        {
            Debug.Log("Destory weed");
            //Destroy(origin.gameObject, 10f);
            //OnTriggerExit doesn't fire if the collider is disabled or destroyed, so I'm taking the hacky way and moving it into the void before destroying it a split second later).
            //transform.parent.position = new Vector3(9999f, 9999f, 9999f);
            //Destroy(transform.parent.gameObject, .1f);
        }
    }
}