using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu_ChangeMenu : MonoBehaviour {
    public GameObject SettingsMenu;
    public GameObject CreditsMenu;
    public GameObject MainMenu;

	public void ToggleSettings()
    {
        if(SettingsMenu.activeSelf)
        {
            SettingsMenu.SetActive(false);
            MainMenu.SetActive(true);
        }
        else
        {
            MainMenu.SetActive(false);
            SettingsMenu.SetActive(true);
        }
    }

    public void ToggleCredits()
    {
        if (CreditsMenu.activeSelf)
        {
            CreditsMenu.SetActive(false);
            MainMenu.SetActive(true);
        }
        else
        {
            MainMenu.SetActive(false);
            CreditsMenu.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
