using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateAcc : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ConfirmButton()
    {
        SceneManager.LoadScene("Scenes/LoginScene");
    }

    public void GoBackButton()
    {
        SceneManager.LoadScene("Scenes/TitleScene");
    }
}
