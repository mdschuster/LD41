using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Debug.Log(Camera.main.pixelHeight + " " + Camera.main.pixelWidth);
        //Screen.SetResolution(645, 652, false);
    }
	
    public void onClickPlay() {
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadScene("_SCENE_");

    }

    public void onClickExit() {
        Application.Quit();
    }
}
