using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour {

	public void LoadOutScreen()
    {
        SceneManager.LoadScene("Scenes/LoadOut");
    }
    public void Play()
    {
        SceneManager.LoadScene("Scenes/MapOne");
    }
    public void LogOut()
    {
        SceneManager.LoadScene("Scenes/TitleScene");
    }
}
