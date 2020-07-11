using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
	void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag) {
			case "Player":
				print("BOOM!");
				Destroy(gameObject);
				break;
		}
	}
}
