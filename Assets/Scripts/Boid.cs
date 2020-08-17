using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boid : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Fastest possible speed this plant can move, also it's target/default speed.")]
    protected float _maxSpeed = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    protected float _turningSpeed = .5f;

    [SerializeField]
    [Tooltip("This is a radius around the plant that it tries to avoid. The larger the number the sooner the plant begins turning. Does not collide with anything.")]
    protected float _avoidanceRadius = 5f;

    [SerializeField]
    [Tooltip("The max number of things we can avoid at once. Lower to improve speed at the cost of quality. Raise will do the inverse.")]
    protected int _maxColliders = 5;

    [SerializeField]
    [Tooltip("How much effort do we put into avoiding obstacles.")]
    protected float _avoidForce = 2f;

    [SerializeField]
    [Tooltip("The boid will stay at the Y it's set/spawned to.")]
    protected bool _lockHeight = false;

    [SerializeField]
    public Transform _target;

    //The colliders around this plant we are trying to avoid.
    protected Collider[] _collidersInRange;

    protected Rigidbody rb;

    // I kept this private because if they override and don't call the base class it will break the rb variable. 
	private void Awake()
	{
        rb = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	protected virtual void Start()
    {
        _collidersInRange = new Collider[_maxColliders];
    }

    // Update is called once per frame
    protected virtual void Update()
    {        
        rb.velocity = getDesiredVelocity();
        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
    }

    protected Vector3 getDesiredVelocity()
	{
        Vector3 direction = _target.position - transform.position;
        if (_lockHeight)
            direction.y = transform.position.y;
        float distance = direction.magnitude;

        direction /= Mathf.Sqrt(distance);
        direction *= (_maxSpeed * (distance / _avoidanceRadius));
        Vector3 ret = direction;

        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _avoidanceRadius, _collidersInRange);
            
        for (int i = 0; i < numColliders; i++)
        {
            if (_collidersInRange[i].transform == transform || _collidersInRange[i].transform == _target)
                continue; //We don't avoid ourself or the target.

            //for each collider get distance
            direction = transform.position - _collidersInRange[i].transform.position;
            
            // for each collider we're trying to avoid we need to make sure to do it restrained to the our Y axis 2D plane. This was added for the cloud to stay in the air.
            if (_lockHeight)
                direction.y = transform.position.y;
            
            float proximityWeight = 1 - (direction.magnitude / _avoidanceRadius); //percent of the way to obsticle 
                
            ret += direction.normalized * proximityWeight * _avoidForce;
        }

        //return a percentage of the desired rotation (the max turn the boid is allowed to make).
        return Vector3.Slerp(ret.normalized * _maxSpeed, transform.rotation.eulerAngles, _turningSpeed * Time.deltaTime);
    }
}
