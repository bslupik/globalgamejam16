﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Level : Base, IObservable<int>
{
    public const float BEAT_THRESHOLD = 0.5f;
    public float timeSinceBeat = 0.0f;
    public float timePerBeat = 0.5357f;
    public float levelScore = 0.0f;
    public float levelScoreBuffer = 0;
    public float levelTime = 0.0f;
    public float maxLevelTime = 33.0f;
    public WorldManager worldManager;
    public float localTime = 0f;
    public float totalScoreMult = 1f;
    public int mazeNodeCount = 21;
    public int writingLineCount = 10;
    public int fishCaught = 0;
    public int circlesDrawn = 0;
    public int firesExtinguished = 0;
    public int dodosCaught = 0;

    public float dodoScoreValue = 1f;
    public float ghostFailValue = 0.5f;
    public float ghostScoreValue = 1f;
    public float villagerFailValue = 1f;
    public float villagerScoreValue = 1f;
    public float lumberFailValue = 1f;
    public float lumberScoreValue = 1f;
    public float torchScoreValue = 1f;
    public float acupunctureScoreValue = 1f;
    public float acupunctureFailValue = 1f;
    public float mazeScoreValue = 1f;
    public float writingScoreValue = 1f;
    public float cauldronScoreValue = 1f;

    // public Text scoreText;

    Observable<int> orderObservable = new Observable<int>();

    public Observable<int> Observable(IObservable<int> self) { return orderObservable; }

    [SerializeField]
    public int[] orderedNumbers;

    [SerializeField]
    protected float shakeMagnitude;

    [SerializeField]
    protected float shakeDuration;

    LinkedList<int> sortedOrderedNumbers;
    List<IResettable> completedObjects = new List<IResettable>();

    List<Collider2D> overlappingColliders = new List<Collider2D>();

	public override void Start()
	{
        base.Start();
        worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
        Array.Sort(orderedNumbers);
        sortedOrderedNumbers = new LinkedList<int>(orderedNumbers);
        Debug.Log(sortedOrderedNumbers);
        sound.PlayBackground(worldManager.musicIndex[worldManager.currentLevelIndex]);
        if (maxLevelTime > 29 && maxLevelTime < 31)
        {
            maxLevelTime = 33.0f;
        }
        timePerBeat = 0.5357f;
        // scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }
	
	public override void Update()
	{
        base.Update();
        timeSinceBeat += Time.deltaTime;
        if (timeSinceBeat > timePerBeat)
        {
            //Debug.Log("BEAT");
            timeSinceBeat -= timePerBeat;
        }
        localTime += Time.deltaTime;
        // scoreText.text = "" + levelScore;
        levelTime += Time.deltaTime;
        if (levelTime >= maxLevelTime)
        {
            EndLevel();
        }
    }

    public float ScoreMultiplier()
    {
        float clampedTimeSinceBeat = timeSinceBeat / timePerBeat;

        return 1.0f - 2 * Mathf.Max(0.0f, Mathf.Min(clampedTimeSinceBeat, 1.0f - clampedTimeSinceBeat));
    }

    public bool OnBeat()
    {
        //Debug.Log(ScoreMultiplier());
        return ScoreMultiplier() >= BEAT_THRESHOLD;
    }

    public void PlayBaseActionSound(int actionIndex)
    {
        if (ScoreMultiplier() < 0.5f)
        {
            sound.PlayActionSound(actionIndex, 0);
        }
        else if (ScoreMultiplier() < 0.75f)
        {
            sound.PlayActionSound(actionIndex, 1);
        }
        else
        {
            sound.PlayActionSound(actionIndex, 2);
        }
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
        PlayBaseActionSound(3);
        Debug.Log(sortedOrderedNumbers);
        if (overlappingColliders.Contains(food.Collider))
        {
            if (food.order == nextOrderedNumber())
            {
                sortedOrderedNumbers.RemoveFirst();
                PlayerActed();
                orderObservable.Post(food.order + 3);
                return true;
            }
            else
            {
                food.reset();
                overlappingColliders.Clear();
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

            if (node.order == nextOrderedNumber())
            {
                sortedOrderedNumbers.RemoveFirst();
                if (target.order == nextOrderedNumber())
                {
                    completedObjects.Add(node);
                    PlayerActed();
                    return true;
                }
                else
                {
                    sortedOrderedNumbers.AddFirst(node.order);
                }
            }
            ScreenShake();
            //Debug.Log("a");
            return false;
        }
        else
        {
            //Debug.Log("b");
            ScreenShake();
            return false;
        }
    }

    public bool PlayerActed(int order)
    {
        if (order == nextOrderedNumber())
        {
            sortedOrderedNumbers.RemoveFirst();
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

    // ==================== Dodo Shooting ==================== //
    public void DodoKilled()
    {
        levelScore += dodoScoreValue;
        PlayBaseActionSound(12);
    }

    // ==================== Graveyard ==================== //
    public void GhostEscaped()
    {
        levelScore -= ghostFailValue;
    }

    public void GhostClicked()
    {
        levelScore += level.ScoreMultiplier() * ghostScoreValue;
        PlayBaseActionSound(15);
    }

    // ==================== Village Raid ==================== //
    public void EnemyGotThrough()
    {
        levelScore -= 1f;
    }

    public void EnemySlashed()
    {
        levelScore += level.ScoreMultiplier() * villagerFailValue;
        PlayBaseActionSound(5);
    }

    // ==================== When I'm Chopping Lumber ==================== //
    public void Chopped(int type)
    {
        switch (type)
        {
            case 1:
                levelScore -= lumberFailValue;
                PlayBaseActionSound(7);
                break;
            case 0:
                levelScore += level.ScoreMultiplier() * lumberScoreValue;
                PlayBaseActionSound(9);
                break;
        }
    }

    // ==================== Tiki Torches ==================== //
    public void TorchClicked()
    {
        levelScore += level.ScoreMultiplier() * torchScoreValue;
        PlayBaseActionSound(6);
    }

    // ==================== Acupuncture ==================== //
    public void AcupuncturePinned()
    {
        levelScore += level.ScoreMultiplier() * acupunctureScoreValue;
    }

    public void AcupunctureMiss()
    {
        levelScore -= acupunctureFailValue;
        PlayBaseActionSound(8);
    }

    // ==================== Maze ==================== //
    public void NodeReached()
    {
        totalScoreMult += level.ScoreMultiplier() * mazeScoreValue;
    }

    // ==================== Writing ==================== // 
    public void LineDrawn()
    {
        totalScoreMult += level.ScoreMultiplier() * writingScoreValue;
        PlayBaseActionSound(11);
    }

    // ==================== Cauldron ==================== //
    public void RecipeCompleted()
    {
        levelScore += cauldronScoreValue;
    }

    // ==================== Putting out Fires ==================== //
    public void ExtinguishArea() // Call whenever you draw a circle
    {
        levelScore += level.ScoreMultiplier();
        circlesDrawn++;
    }

    public void ExtinguishFire(int numfires) // Call when you extinguish Fires in circle
    {
        firesExtinguished += numfires;
        PlayBaseActionSound(3);
    }

    // ==================== NetFishing ==================== //
    public void HerdFish() // Call whenever you draw a circle
    {
        totalScoreMult += level.ScoreMultiplier();
        circlesDrawn++;
    }
    public void CatchFish(int numfish) // Call when the net catches fish
    {
        fishCaught += numfish;
        PlayBaseActionSound(3);
    }

    // ==================== Lasso ==================== //
    public void HerdDodos() // Call whenever you draw a circle
    {
        totalScoreMult += level.ScoreMultiplier();
        circlesDrawn++;
    }
    public void CatchDodo(int numDodos) // Call when you catch Dodos in circle
    {
        dodosCaught += numDodos;
        PlayBaseActionSound(13);
    }
    // =============================================== //

    public void EndLevel()
    {
        switch( GameObject.Find("WorldManager").GetComponent<WorldManager>().currentLevelIndex )
        {
            case 5:
                levelScore = (totalScoreMult / mazeNodeCount) * localTime;
                break;
            case 8:
                levelScore = (totalScoreMult / writingLineCount) * localTime;
                break;
            case 9:
                levelScore = (totalScoreMult / circlesDrawn) * fishCaught;
                break;
            case 10:
                levelScore = (totalScoreMult / circlesDrawn) * dodosCaught;
                break;
            case 11:
                levelScore = (totalScoreMult / circlesDrawn) * firesExtinguished;
                break;
        }
        GameObject.Find("WorldManager").GetComponent<WorldManager>().EndLevel(levelScore);
    }

    private int nextOrderedNumber()
    {
        if (sortedOrderedNumbers.First == null)
        {
            return -1;
        }
        return sortedOrderedNumbers.First.Value;
    }
}


public interface IResettable
{
    void reset();
}