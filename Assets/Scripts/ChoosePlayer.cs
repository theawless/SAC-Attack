using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoosePlayer : MonoBehaviour {

	bool IsPlayer1Selected = false;
	bool IsPlayer2Selected = false;

	float z_Selected = -50f;

	public GameObject Player_1;
	public GameObject Player_2;

	GameObject Player1DropImage ;
	GameObject Player2DropImage;

	public bool player1Selected = false;
	//bool player2Selected = false;

	GameInfo gameInfo;
	GameObject gameInfoGo;


	// Use this for initialization
	void Start () {
		if (!Player_1 || !Player_2)
			return;

		Player1DropImage = DropImageGO (Player_1);
		Player2DropImage = DropImageGO (Player_2);

		IsPlayer1Selected = true;

	}
	
	// Update is called once per frame
	void Update () {
		gameInfoGo = GameObject.FindGameObjectWithTag("GameInfo");
		gameInfo = gameInfoGo.GetComponent<GameInfo>();

		SelectedMagnification (Player_1, IsPlayer1Selected, Player1DropImage);
		SelectedMagnification (Player_2, IsPlayer2Selected,	Player2DropImage);
	}

	void SelectedMagnification(GameObject gobj, bool IsSelected, GameObject DropImageSelector)
	{
		RectTransform rect_trans = gobj.GetComponent<RectTransform> ();
		Vector3 temp = rect_trans.anchoredPosition3D;
		if (IsSelected) {
			temp.z = z_Selected;
			rect_trans.anchoredPosition3D = temp;
			gobj.GetComponent<Image> ().color = new Color (255, 255, 255, 255);
			SelectPlayer (DropImageSelector);
		} else {
			temp.z = 0;
			rect_trans.anchoredPosition3D = temp;
			gobj.GetComponent<Image> ().color = new Color (0, 44, 255, 146);    //correctionTOBeDone
		}
	}

	void SelectPlayer(GameObject go){
		var anims = gameObject.GetComponentsInChildren<Animator> ();
		foreach (var anim in anims) {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Highlighted")) {
				//Debug.Log (anim.gameObject.name);
				GameObject temp = DropImageGO (anim.gameObject);
				go.GetComponent<Image> ().sprite = temp.GetComponent<Image> ().sprite;
			}
		}
	}

	GameObject DropImageGO (GameObject go)
	{
		GameObject gobj = null;
		var gobjs = go.GetComponentsInChildren<Image> ();
		foreach (var gob in gobjs) {
			if (gob.gameObject.name == "Drop Image")
				gobj = gob.gameObject;
				//Debug.Log (gob.sprite);
			}
		return gobj;
	}

	public void Selected(int charNum){
		if (player1Selected) {
			gameInfo.P2Character = charNum;
			SceneManager.LoadScene (2);
		}
			//Application.loadedLevel
		else{
			gameInfo.P1Character = charNum;
			player1Selected = true;
			IsPlayer1Selected = false;
			IsPlayer2Selected = true;
		}
	}
}
