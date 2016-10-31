using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour {

	GameInfo gameInfo;
	GameObject gameInfoGo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
		gameInfoGo = GameObject.FindGameObjectWithTag("GameInfo");
		gameInfo = gameInfoGo.GetComponent<GameInfo>();
	}

	public void MapSelection(int mapNum){
		if (mapNum == 1)
			gameInfo.NewSac = true;
		else if (mapNum == 2)
			gameInfo.FoodCourt = true;
		else
			return;
		AudioSource audio = gameInfoGo.GetComponent<AudioSource> ();
		audio.Stop ();
        if (gameInfo.NewSac)
            SceneManager.LoadScene(3);
        else if (gameInfo.FoodCourt)
            SceneManager.LoadScene(4);
	}
}
