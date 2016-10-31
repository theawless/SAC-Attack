using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour {

	public bool AIMode = false;
	public bool vsMode = false;

	public int P1Character = -1;
	public int P2Character = -1;

	public bool NewSac = false;
	public bool FoodCourt = false;

	ChoosePlayer player ;
	GameObject characterController;

	// Use this for initialization
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnLevelLoaded(){
		characterController = GameObject.FindGameObjectWithTag ("CharacterController");
		player = characterController.GetComponent<ChoosePlayer> ();
	}

	public void ModeSelection(int ModeNum){
		if (ModeNum == 1) {
			AIMode = true;
		} else if (ModeNum == 2) {
			vsMode = true;
		} 
		else
			return;
		SceneManager.LoadScene (1);
	}
		
}
