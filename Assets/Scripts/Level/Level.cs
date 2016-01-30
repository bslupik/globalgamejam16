using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level : Base
{
    public float timeSinceBeat = 0.0f;
    public float timePerBeat = 1.0f;
    public float levelScore = 0.0f;
    public Text scoreText;

	public override void Start()
	{
        base.Start();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }
	
	public override void Update()
	{
        base.Update();
        timeSinceBeat += Time.deltaTime;
        if (timeSinceBeat > timePerBeat)
        {
            Debug.Log("BEAT");
            timeSinceBeat -= timePerBeat;
        }

        scoreText.text = "" + levelScore;
    }

    public float ScoreMultiplier()
    {

        float clampedTimeSinceBeat = timeSinceBeat / timePerBeat;

        // 1.1 so that we don't go negative.
        return 1.1f - Mathf.Min(clampedTimeSinceBeat, 1 - clampedTimeSinceBeat);
    }

    public void PlayerActed(float score = 1.0f)
    {
        // Play a sound here.
        float multiplier = ScoreMultiplier();
        levelScore += score * multiplier;
    }

    public void GhostEscaped()
    {
        // Play a sound here
    }
}
