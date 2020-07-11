using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corn : MonoBehaviour
{
    public bool Can_attack = false;
    public float attack_interval = 2;
    float attack_timer;
    public GameObject attack_missile;
    GameObject player;

    public int growthstate2startingpoint;

    public int growthstate3startingpoint;

    public int currentgrowthstate;

    public float currentgrowth;

    public float growth_coeficient;
    public Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentgrowth = growth_coeficient/2;
        attack_timer = attack_interval;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<health>() != null && Can_attack == true)
        {
            collision.gameObject.GetComponent<health>().HP_now -= 25;
        }
  
    }
    // Update is called once per frame
    void Update()
    {
        tr.localScale = new Vector3(currentgrowth / growth_coeficient, currentgrowth / growth_coeficient, currentgrowth / growth_coeficient);
        tr = gameObject.GetComponent<Transform>();

        if (currentgrowth < growthstate3startingpoint)
        {
            currentgrowth++;
        }

        if (currentgrowth > growthstate2startingpoint && currentgrowth < growthstate3startingpoint)
        {
            currentgrowthstate = 2;
        }
        if (currentgrowth + 1 > growthstate3startingpoint)
        {
            currentgrowthstate = 3;
        }
        if (currentgrowthstate == 3)
        {
            Can_attack = true;
        }
        if (Can_attack && player)
        {
            attack_timer -= Time.deltaTime;
            if (attack_timer <= 0)
            {
                attack_timer += attack_interval;
                Vector3 direction = (player.transform.position - transform.position);
                direction.y = 0;
                direction.Normalize();
                GameObject missile = Instantiate(attack_missile, transform.position+direction, transform.rotation);
                Physics.IgnoreCollision(GetComponent<Collider>(), missile.GetComponent<Collider>());
                Boid boid = missile.GetComponent<Boid>();
                boid._target = player.transform;
            }
        }
    }
}
