/*NOT USED IN PROJECT*/
using UnityEngine;
using System.Collections;

public class DropCube : MonoBehaviour {

	public Rigidbody rigidbody;

	void Start() {
		rigidbody = gameObject.GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Cube") {
			Destroy(other.rigidbody);
		}
	}
}
