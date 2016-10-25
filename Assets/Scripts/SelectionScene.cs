using UnityEngine;
using UnityEngine.UI;

public class SelectionScene : MonoBehaviour {

	//panel references
	public GameObject panel_SceneSelection;
	public GameObject panel_Register;
	public GameObject panel_SignIn;

	//minor UI references
	public InputField passwordField_SignIn;
	public InputField passwordField_Register;
	public string pwd;

	//animation controller for animations
	public GameObject animatorGO;
	private Animator animator;

	void Awake() {
		//panel_Register.SetActive(false);
		//panel_SignIn.SetActive(false); 
	}

	void Start() {
		animator = gameObject.GetComponent<Animator>();
    }

	/*UI Callbacks*/
	public void ChildScene() {
		//Application.LoadLevel("Second");
		animator.SetBool("hasSceneSelected", true);
    }	

	public void ParentScene() {
		//Application.LoadLevel("Third");
		animator.SetBool("hasSceneSelected", true);
	}

	public void Register() {
		pwd = passwordField_Register.text;
		if (pwd != null) {
			PlayerPrefs.SetString("password", pwd);
			Debug.Log(PlayerPrefs.GetString("password"));
			Debug.Log("password saved successfully");
		}
	}

	public void SignIn() {
		string input;
		input = passwordField_SignIn.text;
		if (input != PlayerPrefs.GetString("password")) {
			Debug.Log("access denied");
		} else {
			Debug.Log("good to go");
		}
	}
}
