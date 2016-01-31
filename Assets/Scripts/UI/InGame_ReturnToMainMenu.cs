using UnityEngine;
using System.Collections;

public class InGame_ReturnToMainMenu : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        Application.LoadLevel(0);
    }

    public void GoToNextLevel()
    {
        GameObject.FindWithTag("LevelManager").GetComponent<Level>().EndLevel();
    }
}
