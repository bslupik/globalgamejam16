using UnityEngine;
using System.Collections;

public class NSpawnCondition : SpawnCondition {

    public int maxObjects;
    public GameObject[] objects;

    public override void Start () {
		base.Start();
        objects = new GameObject[maxObjects];
	}
	
	public override void Update () {
		base.Update();
	}

    public override bool ShouldSpawn()
    {
        return numObjects() < maxObjects;
    }

    public override void OnSpawn(GameObject spawnObject)
    {
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
