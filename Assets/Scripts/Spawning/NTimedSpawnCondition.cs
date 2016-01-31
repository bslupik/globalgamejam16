using UnityEngine;
using System.Collections;

public class NTimedSpawnCondition : TimedSpawnCondition {

    public int maxObjects;
    public GameObject[] objects;

    public override void Start()
    {
        base.Start();
        objects = new GameObject[maxObjects];
    }

    public override void Update()
    {
        if (numObjects() == maxObjects)
        {
            timeSinceSpawn = 0.0f;
            return;
        }

        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn >= spawnTime)
        {
            timeSinceSpawn -= spawnTime;
            shouldSpawn = true;
        }
    }

    public override bool ShouldSpawn()
    {
        return base.shouldSpawn && (numObjects() < maxObjects);
    }

    public override void OnSpawn(GameObject spawnObject)
    {
        base.OnSpawn(spawnObject);
        for (int i = 0; i < maxObjects; ++i)
        {
            if (objects[i] == null)
            {
                objects[i] = spawnObject;
                return;
            }
        }
    }

    private int numObjects()
    {
        int count = 0;
        for (int i = 0; i < maxObjects; ++i)
        {
            if (objects[i] != null)
            {
                ++count;
            }
        }
        return count;
    }
}
