using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteraction : MonoBehaviour
{
    public bool active = true;

    protected Player _player;

	private void Awake()
	{
        _player = GameObject.FindObjectOfType<Player>();

        if (_player == null)
            Debug.LogError("Player not found in scene");

	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Interact();

    void OnDrawGizmos()
    {
        //Set the color
        Gizmos.color = active ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
