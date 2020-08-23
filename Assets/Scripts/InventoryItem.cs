using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryItemStatePair
{
    /// <summary>
    /// The gameobject of the item itself
    /// </summary>
    public GameObject inventoryItem;
    public enum PlayerState { 
        MixedSeeds, 
        CornSeeds, 
        //WatermelonSeeds, 
        //RaddishSeeds,
        Watering,
        Fertilizer
        //Wheelbarrow //this will be an internal state as the wheelbarrow is just set in the world.
    }

    public PlayerState playerState;
}

public class InventoryItem : MonoBehaviour
{
    InventoryItemStatePair stateItemPair;
    public int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}