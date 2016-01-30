using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Level : Base
{
    public const float BEAT_THRESHOLD = 0.5f;
    public float timeSinceBeat = 0.0f;
    public float timePerBeat = 1.0f;
    public float levelScore = 0.0f;
    // public Text scoreText;

    [SerializeField]
    protected int[] foodNumbers;

    Queue<int> sortedFoodNumbers;
    List<FoodDraggable> completedFood = new List<FoodDraggable>();

    List<Collider2D> overlappingColliders = new List<Collider2D>();

    void Awake()
    {
        Array.Sort(foodNumbers);
        sortedFoodNumbers = new Queue<int>(foodNumbers);
    }

	public override void Start()
	{
        base.Start();
        // scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
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

        // scoreText.text = "" + levelScore;
    }

    public float ScoreMultiplier()
    {
        float clampedTimeSinceBeat = timeSinceBeat / timePerBeat;

        return 1.0f - 2 * Mathf.Max(0.0f, Mathf.Min(clampedTimeSinceBeat, 1.0f - clampedTimeSinceBeat));
    }

    public bool OnBeat()
    {
        return ScoreMultiplier() >= BEAT_THRESHOLD;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        overlappingColliders.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        overlappingColliders.Remove(other);
    }

    public bool PlayerActed(FoodDraggable food)
    {
        Debug.Log("FOOD");
        if (overlappingColliders.Contains(food.Collider))
        {
            Debug.Log("Food Contains");
            if (food.order == sortedFoodNumbers.Peek())
            {
                sortedFoodNumbers.Dequeue();
                PlayerActed();
                completedFood.Add(food);
                return true;
            }
            else
            {

                for (int i = 0; i < completedFood.Count; i++)
                {
                    completedFood[i].reset();
                }
                food.reset();
                completedFood.Clear();
                return false;
            }
        }
        return false;
    }
}
