using UnityEngine;

public class DisableAdditionalComponents : MonoBehaviour {

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "DropCubes" || other.gameObject.tag == "Cube") {
			Destroy(gameObject.GetComponent<Rigidbody>());
			Debug.Log("adadad");
		}
	}
}
