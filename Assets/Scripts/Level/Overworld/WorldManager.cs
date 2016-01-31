using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public GameObject persistentUI;

    public bool isEndlessMode;
    public bool endlessShuffle;
    public int[] endlessSequence;
    public int endlessIndex;

    public void Start()
	{
        levelFinish = GameObject.Find("LevelFinishUI");
        instructionScreen = GameObject.Find("InstructionScreen");
        persistentUI = GameObject.Find("PersistentUI");
        levelFinish.SetActive(false);
        instructionScreen.SetActive(false);
        persistentUI.SetActive(false);
        for (int i = 0; i < village.Length; ++i)
        {
            if (difficulty[i] == 0)
            {
                villages[village[i]].easy.Add(i);
            }
            else if (difficulty[i] == 1)
            {
                villages[village[i]].medium.Add(i);
            }
            else
            {
                villages[village[i]].hard.Add(i);
            }
        }
        scripting = Camera.main.GetComponent<SceneChangeScripting>();

        InitEndlessMode();
    }

    public void InitEndlessMode()
    {
        isEndlessMode = false;
        string endlessSequenceString = PlayerPrefs.GetString("EndlessLevelSequence");
        if (endlessSequenceString != null && endlessSequenceString.Length > 0)
        {
            isEndlessMode = true;
            string[] endlessSequenceStringArray = endlessSequenceString.Split(' ');
            endlessSequence = new int[endlessSequenceStringArray.Length];
            for (int i = 0; i < endlessSequenceStringArray.Length; ++i)
            {
                endlessSequence[i] = int.Parse(endlessSequenceStringArray[i]);
            }
        }
        string endlessShuffleString = PlayerPrefs.GetString("EndlessLevelShuffle");
        if (endlessShuffleString != null)
        {
            endlessShuffle = bool.Parse(endlessShuffleString);
        }
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
        instructionScreen.GetComponentInChildren<Image>().sprite = instructionBackground[currentLevelIndex];
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
        persistentUI.SetActive(true);
    }

    public void EndLevel(float finalScore)
    {
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
        persistentUI.SetActive(false);
        OnLevelFinished(finalScore);
        yield return scripting.FadeIn();
        Time.timeScale = 1.0f;
    }

    public void CloseLevelFinish()
    {
        if(isEndlessMode)
        {
            SetNextEndlessLevel();
            StartCoroutine(FadeToLevel());
        }
        else
        {
            StartCoroutine(FadeToOverworld());
        }
    }

    public void SetNextEndlessLevel()
    {
        if(endlessShuffle)
            endlessIndex = Random.Range(0, endlessSequence.Length);
        else
            endlessIndex = (endlessIndex + 1) % endlessSequence.Length;

        currentLevelIndex = endlessSequence[endlessIndex];
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
