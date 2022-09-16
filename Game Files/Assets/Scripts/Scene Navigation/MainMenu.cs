using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        User u1 = new User("35.197.31.116", 9252);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoToLoginScreen()
    {
        SceneManager.LoadScene("Scenes/LoginScene");
    }

    public void GoToRegisterScreen()
    {
        SceneManager.LoadScene("Scenes/RegistrationScene");
    }
}
