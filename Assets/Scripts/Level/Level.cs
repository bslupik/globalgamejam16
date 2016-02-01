using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Level : Base, IObservable<int>
{
    public const float BEAT_THRESHOLD = 0.5f;
    public float timeSinceBeat = 0.0f;
    public float timePerBeat = 1.0f;
    public float levelScore = 0.0f;
    public float levelScoreBuffer = 0;
    // public Text scoreText;

    Observable<int> orderObservable = new Observable<int>();

    public Observable<int> Observable(IObservable<int> self) { return orderObservable; }

    [SerializeField]
    public int[] orderedNumbers;

    [SerializeField]
    protected float shakeMagnitude;

    [SerializeField]
    protected float shakeDuration;

    Queue<int> sortedOrderedNumbers;
    List<IResettable> completedObjects = new List<IResettable>();

    List<Collider2D> overlappingColliders = new List<Collider2D>();

	public override void Start()
	{
        base.Start();
        Array.Sort(orderedNumbers);
        sortedOrderedNumbers = new Queue<int>(orderedNumbers);
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
        Debug.Log(ScoreMultiplier());
        return ScoreMultiplier() >= BEAT_THRESHOLD;
    }

    public void PlayerActed(float score = 1.0f)
    {
        // Play a sound here.
        float multiplier = ScoreMultiplier();
        levelScore += score * multiplier;
    }

    public void FlushScoreBuffer()
    {
        PlayerActed(levelScoreBuffer);
        levelScoreBuffer = 0;
    }

    public void PlayerActedBuffer(float score = 1.0f)
    {
        levelScoreBuffer += score;
    }

    public void ScreenShake()
    {
        Callback.DoLerp((float l) => Camera.main.transform.localPosition = UnityEngine.Random.insideUnitCircle * shakeMagnitude * l, shakeDuration, this, reverse: true);
    }

    // ==================== Graveyard ==================== //
    public void GhostEscaped()
    {
        // Play a sound here.
    }

    // ==================== Village Raid ==================== //
    public void EnemyGotThrough()
    {
        // Play a sound here.
    }

    // ==================== When I'm Chopping Lumber ==================== //
    public void Chopped(int type)
    {
        // Play a sound here.
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
        if (overlappingColliders.Contains(food.Collider))
        {
            if (food.order == sortedOrderedNumbers.Peek())
            {
                sortedOrderedNumbers.Dequeue();
                PlayerActed();
                completedObjects.Add(food);
                orderObservable.Post(food.order + 3);
                return true;
            }
            else
            {

                for (int i = 0; i < completedObjects.Count; i++)
                {
                    completedObjects[i].reset();
                }
                food.reset();
                completedObjects.Clear();
                overlappingColliders.Clear();
                sortedOrderedNumbers = new Queue<int>(orderedNumbers);
                ScreenShake();
                return false;
            }
        }
        else
        {
            food.reset();
        }
        return false;
    }

    public bool PlayerActed(MazeNode node)
    {
        MazeNode target = null;


        Collider2D[] targets = Physics2D.OverlapPointAll(Draggable.mousePosInWorld());
        for(int i = 0; i < targets.Length; i++)
        {
            target = targets[i].transform.GetComponent<MazeNode>();
            if (target != null)
                break;
        }

        if (target != null)
        {

            if (node.order == sortedOrderedNumbers.Peek())
            {
                Debug.Log("c");
                completedObjects.Add(node);
                sortedOrderedNumbers.Dequeue();
                Debug.Log(target.order + " " + sortedOrderedNumbers.Peek());
                if (target.order == sortedOrderedNumbers.Peek())
                {
                    PlayerActed();
                    return true;
                }
            }
            ScreenShake();
            completedObjects.Clear();
            sortedOrderedNumbers = new Queue<int>(orderedNumbers);
            Debug.Log("a");
            return false;
        }
        else
        {
            Debug.Log("b");
            ScreenShake();
            return false;
        }
    }

    public bool PlayerActed(int order)
    {
        if (order == sortedOrderedNumbers.Peek())
        {
            sortedOrderedNumbers.Dequeue();
            PlayerActed();
            if (sortedOrderedNumbers.Count == 0)
            {
                EndLevel();
            }
            return true;
        }
        else
        {
            ScreenShake();
            return false;
        }
    }

    public void ArrowMissed()
    {
        // Do something maybe
    }

    public void DodoKilled()
    {
        // Play sound, give points
    }

    public void EndLevel()
    {
        GameObject.Find("WorldManager").GetComponent<WorldManager>().EndLevel(levelScore);
    }
}


public interface IResettable
{
    void reset();
}