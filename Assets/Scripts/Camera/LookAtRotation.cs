/*NOT USED IN PROJECT*/
using UnityEngine;

public class LookAtRotation : MonoBehaviour {
	public GameObject centerGameobject;
	void Update() {
		transform.RotateAround(centerGameobject.transform.position, Vector3.up, 0.2f);
	}
}
