using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public int numMiniGames = 0;
    public int numStars = 0;

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
    public GameObject gameEndingUI;
    public Sprite[] gameEnds;

    public bool isEndlessMode;
    public bool endlessShuffle;
    public int[] endlessSequence;
    public int endlessIndex;

    public void Start()
	{
        levelFinish = GameObject.Find("LevelFinishUI");
        instructionScreen = GameObject.Find("InstructionScreen");
        persistentUI = GameObject.Find("PersistentUI");
        gameEndingUI = GameObject.Find("GameEndingUI");
        levelFinish.SetActive(false);
        instructionScreen.SetActive(false);
        persistentUI.SetActive(false);
        gameEndingUI.SetActive(false);
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
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayBackground(4);
        InitEndlessMode();
    }

    public void InitEndlessMode()
    {
        isEndlessMode = false;
        string endlessSequenceString = PlayerPrefs.GetString("EndlessLevelSequence", "");
        if (endlessSequenceString != "" && endlessSequenceString.Length > 0)
        {
            isEndlessMode = true;
            string[] endlessSequenceStringArray = endlessSequenceString.Split(' ');
            endlessSequence = new int[endlessSequenceStringArray.Length];
            for (int i = 0; i < endlessSequenceStringArray.Length; ++i)
            {
                endlessSequence[i] = int.Parse(endlessSequenceStringArray[i]);
            }
        }
        string endlessShuffleString = PlayerPrefs.GetString("EndlessLevelShuffle", "");
        if (endlessShuffleString != "")
        {
            endlessShuffle = bool.Parse(endlessShuffleString);
        }
    }
	
	public void Update()
	{
	}

    public void StartLevel(int index)
    {
        ++numMiniGames;
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
        for (int i = 0; i < villages.Length; ++i)
        {
            villages[i].gameObject.SetActive(false);
        }
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
            villages[i].gameObject.SetActive(false);
        }

        StartCoroutine(FadeToLevelFinish(finalScore));
    }
    
    public IEnumerator FadeToLevelFinish(float finalScore)
    {
        yield return scripting.FadeOut();
        Time.timeScale = 0;
        GameObject.FindWithTag("SaveManager").GetComponent<SaveManager>().UnloadLevel();
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayBackground(4);
        persistentUI.SetActive(false);
        OnLevelFinished(finalScore);
        yield return scripting.FadeIn();
        Time.timeScale = 1.0f;
    }

    public void ShowEnding()
    {
        levelFinish.SetActive(false);
        int finish = 0;
        if (numStars >= 8)
        {
            finish = 1;
        }

        if (numStars >= 14)
        {
            finish = 2;
        }
        gameEndingUI.GetComponentInChildren<Image>().sprite = gameEnds[finish];
        gameEndingUI.SetActive(true);
    }

    public void CloseLevelFinish()
    {
        if (numMiniGames >= 5)
        {
            ShowEnding();
            return;
        }

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
        for (int i = 0; i < villages.Length; ++i)
        {
            villages[i].gameObject.SetActive(false);
        }
        villages[Random.Range(0, villages.Length)].gameObject.SetActive(true);
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
            ++numStars;
        }

        if (score >= greatScore[currentLevelIndex])
        {
            GameObject.Find("ThirdStar").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            ++numStars;
        }

        if (score >= goodScore[currentLevelIndex])
        {
            GameObject.Find("SecondStar").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            ++numStars;
        }
    }
}
