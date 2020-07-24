using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInteraction : PlayerInteraction
{
    /// <summary>
    /// +1 on trigger enter, -1 on trigger exit
    /// </summary>
    int _count;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnTriggerEnter(Collider other)
	{
        _count++;
        active = false;
	}

	public void OnTriggerExit(Collider other)
	{
        _count--;

        if (_count <= 0)
            active = true;
	}

	void OnDrawGizmos()
	{
        //Set the color
        Gizmos.color = active ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
