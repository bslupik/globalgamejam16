using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu_ChangeLevel : MonoBehaviour {

	public void ChangeLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
