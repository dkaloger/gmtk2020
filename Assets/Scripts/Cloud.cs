using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : Boid
{
    ParticleSystem _rainEffect;
    Player _player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!_player)
            _player = FindObjectOfType<Player>();
        _rainEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update is where we get all our movements calculated from. If you change the velocity after this point, it won't have any collision avoidance. 
        base.Update();
    }

    public void ResetTargetToPlayer()
	{
        _target = _player.transform;
        _rainEffect.enableEmission = false;
	}

    public void GoRainAtMousePosition()
	{
		_target = _player.playerInteractionReticle;
        _rainEffect.Play();
        _rainEffect.enableEmission = true;
    }

	public void OnTriggerStay(Collider other)
	{
        if (other.TryGetComponent<Plant>(out Plant plant))
            plant.Water();
	}
}
