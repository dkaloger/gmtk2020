using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    /// <summary>
    /// How the player interacts with the world. This function will be called for the entire time the player is holding the button
    /// </summary>
    /// <param name="inputValue">onFire input constrained from 0 not pressed to 1 completely pressed</param>
    public abstract void Interact(float inputValue);

    void OnDrawGizmos()
    {
        //Set the color
        Gizmos.color = active ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }
}
