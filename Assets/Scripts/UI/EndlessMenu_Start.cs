using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndlessMenu_Start : Base {

    public EndlessMenu_LevelButton[] levelButtons;
    public Toggle shuffleToggle;

	public override void Start () {
		base.Start();
	}
	
	public override void Update () {
		base.Update();
	}

    public void Play()
    {
        PlayerPrefs.SetString("EndlessLevelSequence", getSelectedLevelIndicesString());
        PlayerPrefs.SetString("EndlessLevelShuffle", shuffleToggle.isOn.ToString());
    }

    protected string getSelectedLevelIndicesString()
    {
        string levelIndices = "";
        foreach(EndlessMenu_LevelButton button in levelButtons) {
            if(button.IsSelected())
            {
                levelIndices += button.levelIndex + " ";
            }
        }
        return levelIndices.TrimEnd();
    }
}
