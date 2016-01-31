using UnityEngine;
using System.Collections;

public class WorldManager : Base
{
    public int[] musicIndex;
    public string[] levelName;
    public float[] goodScore;
    public float[] greatScore;
    public float[] perfectScore;
    public int[] difficulty;
    public int[] village;

    public AudioClip[] music;
    public float[] musicBeatSpeed;

    public int levelDifficulty = 1;
    public int currentLevelIndex = -1;

    public Village[] villages;

    public override void Start()
	{
		base.Start();
        for (int i = 0; i < village.Length; ++i)
        {
            if (difficulty[i] == 0)
            {
                villages[i].easy.Add(i);
            }
            else if (difficulty[i] == 0)
            {
                villages[i].medium.Add(i);
            }
            else
            {
                villages[i].hard.Add(i);
            }
        }
	}
	
	public override void Update()
	{
		base.Update();
	}

    public void StartLevel(int index)
    {
        currentLevelIndex = index;
        for (int i = 0; i < villages.Length; ++i)
        {
            villages[i].OnStart();
        }
    }

    public void EndLevel()
    {
        for (int i = 0; i < villages.Length; ++i)
        {
            villages[i].OnUnload();
        }

        GameObject.FindWithTag("SaveManager").GetComponent<SaveManager>().UnloadLevel();
    }
}
