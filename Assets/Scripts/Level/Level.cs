using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level : Base
{
    public float timeSinceBeat = 0.0f;
    public float beatSpeed = 1.0f;
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
        if (timeSinceBeat > beatSpeed)
        {
            timeSinceBeat -= beatSpeed;
        }

        scoreText.text = "" + levelScore;
    }

    public float ScoreMultiplier()
    {
        // 1.1 so that we don't go negative.
        return 1.1f - (timeSinceBeat / beatSpeed);
    }

    public void PlayerActed(float score = 1.0f)
    {
        // Play a sound here.
        float multiplier = ScoreMultiplier();
        print(multiplier);
        print(timeSinceBeat);
        levelScore += score * multiplier;
    }
}
