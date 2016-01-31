using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour
{
    public int[] musicIndex;
    public string[] levelName;
    public float[] goodScore;
    public float[] greatScore;
    public float[] perfectScore;
    public int[] difficulty;
    public int[] village;
    public Sprite[] instructionBackground;

    public AudioClip[] music;
    public float[] musicBeatSpeed;

    public int levelDifficulty = 1;
    public int currentLevelIndex = -1;

    public Village[] villages;
    SceneChangeScripting scripting;

    public GameObject levelFinish;
    public GameObject instructionScreen;

    public void Start()
	{
        levelFinish = GameObject.Find("LevelFinishUI");
        instructionScreen = GameObject.Find("InstructionScreen");
        levelFinish.SetActive(false);
        instructionScreen.SetActive(false);
        for (int i = 0; i < village.Length; ++i)
        {
            if (difficulty[i] == 0)
            {
                villages[i].easy.Add(i);
            }
            else if (difficulty[i] == 1)
            {
                villages[i].medium.Add(i);
            }
            else
            {
                villages[i].hard.Add(i);
            }
        }
        scripting = Camera.main.GetComponent<SceneChangeScripting>();
    }
	
	public void Update()
	{
	}

    public void StartLevel(int index)
    {
        currentLevelIndex = index;
        for (int i = 0; i < villages.Length; ++i)
        {
            villages[i].OnStart();
        }
        StartCoroutine(FadeToInstructions());
    }

    
    public IEnumerator FadeToInstructions()
    {
        yield return scripting.FadeOut();
        Time.timeScale = 0;
        instructionScreen.SetActive(true);
        yield return scripting.FadeIn();
        Time.timeScale = 1.0f;
    }

    public void CloseInstructions()
    {
        StartCoroutine(FadeToLevel());
    }

    public IEnumerator FadeToLevel()
    {
        yield return scripting.FadeOut();
        Time.timeScale = 0;
        GameObject.FindWithTag("SaveManager").GetComponent<SaveManager>().Load(levelName[currentLevelIndex]);
        instructionScreen.SetActive(false);
        yield return scripting.FadeIn();
        Time.timeScale = 1.0f;
    }

    public void EndLevel(float finalScore)
    {
        print("ending!");
        for (int i = 0; i < villages.Length; ++i)
        {
            villages[i].OnUnload();
        }

        StartCoroutine(FadeToLevelFinish(finalScore));
    }
    
    public IEnumerator FadeToLevelFinish(float finalScore)
    {
        yield return scripting.FadeOut();
        Time.timeScale = 0;
        GameObject.FindWithTag("SaveManager").GetComponent<SaveManager>().UnloadLevel();
        OnLevelFinished(finalScore);
        yield return scripting.FadeIn();
        Time.timeScale = 1.0f;
    }

    public void CloseLevelFinish()
    {
        StartCoroutine(FadeToOverworld());
    }

    public IEnumerator FadeToOverworld()
    {
        yield return scripting.FadeOut();
        Time.timeScale = 0;
        levelFinish.SetActive(false);
        yield return scripting.FadeIn();
        Time.timeScale = 1.0f;
    }

    public void OnLevelFinished(float score)
    {
        levelFinish.SetActive(true);
        GameObject.Find("SecondStar").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        GameObject.Find("ThirdStar").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        GameObject.Find("FourthStar").GetComponent<CanvasRenderer>().SetAlpha(0.0f);

        if (score >= perfectScore[currentLevelIndex])
        {
            GameObject.Find("FourthStar").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
        }

        if (score >= greatScore[currentLevelIndex])
        {
            GameObject.Find("ThirdStar").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
        }

        if (score >= goodScore[currentLevelIndex])
        {
            GameObject.Find("SecondStar").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
        }
    }
}
