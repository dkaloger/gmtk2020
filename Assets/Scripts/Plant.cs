using ByTheTale.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlantInteraction))]
public class Plant : MachineBehaviour
{
    protected GameObject _player;

    [Header("Plant Settings")]

    [Range(0f, 1f)]
    public float startingSize = 0f;

    public float timeTillFullyGrown = 30f;

    public uint cashValue = 0;

    [Header("Watering Settings")]

    [Range(0, 1)]
    [Tooltip("0 = 0% chance monster, 0.5 = 50% chance monster, 1 = 100% chance monster")]
    public float corruption = 0f;

    [Tooltip("corruption only increases when watered.")]
    public float corruptionGrowthPerSecond = 0.03f;

    [Range(0f, 1f)]
    [Tooltip("This is additive to it's normal growth-rate")]
    public float growthRateWatering = 0.02f;

	private void Awake()
	{
        _player = GameObject.FindGameObjectWithTag("Player");
    }

	public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }

	public override void OnTriggerStay(Collider collider)
	{
		base.OnTriggerStay(collider);        
	}

	public void Water()
	{
        corruption += corruptionGrowthPerSecond * Time.deltaTime;
        transform.localScale += Vector3.one * growthRateWatering * Time.deltaTime;

        if (IsCurrentState<SeedState>())
		{
            ((SeedState)currentState).Water();
		}
    }

    public override void AddStates()
	{
        AddState<SeedState>();
        AddState<GrowingState>();
        AddState<HarvestState>();
        AddState<MonsterState>();
        AddState<RottenState>();

        SetInitialState<SeedState>();
    }
}
