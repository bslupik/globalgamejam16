using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndlessMenu_LevelButton : MonoBehaviour {

    public Button button;
    public int levelIndex = -1;
    protected bool selected = true;

	public void Start () {
        updateColor();
	}
	
	public void Update () {
	}

    public bool IsSelected()
    {
        return selected;
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public void ToggleSelection()
    {
        selected = !selected;
        updateColor();
    }

    protected void updateColor()
    {
        Color c = button.image.color;
        c.a = selected ? 1.0f : 0.3f;
        button.image.color = c;
    }
}
