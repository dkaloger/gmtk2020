using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignore_co : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "ground")
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
    }w
}
