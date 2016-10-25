using UnityEngine;
using System.Collections;

public class MiscFuntions : MonoBehaviour {

	public void RestartLevel() {
        Application.LoadLevel(Application.loadedLevel);
    }

	public void Exit() {
		Application.Quit();
	}
}
