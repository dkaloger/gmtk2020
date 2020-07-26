using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        //Set the color
        Gizmos.color = active ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
