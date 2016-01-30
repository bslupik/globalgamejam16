using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu_ChangeLevel : MonoBehaviour {

	public void ChangeLevel(int levelindex)
    {
        SceneManager.LoadScene(levelindex);
    }
}
