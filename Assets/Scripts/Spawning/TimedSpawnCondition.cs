using UnityEngine;
using System.Collections;

public class TimedSpawnCondition : SpawnCondition
{
    public float baseSpawnTime;
    public float spawnTimeVariation;
    public float timeSinceSpawn;
    public float spawnTime;
    public bool shouldSpawn;

	public override void Start()
	{
		base.Start();
        TimedSpawnCondition parentCondition = GetComponentInParent<TimedSpawnCondition>();
        if (parentCondition != null)
        {
            baseSpawnTime = parentCondition.baseSpawnTime;
            spawnTimeVariation = parentCondition.spawnTimeVariation;
        }

        if (spawnTime == 0.0f)
        {
            spawnTime = baseSpawnTime + Random.Range(-spawnTimeVariation, spawnTimeVariation);
        }
    }
	
	public override void Update()
	{
		base.Update();
        if (spawnedObject != null)
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
        return base.ShouldSpawn() && shouldSpawn;
    }

    public override void OnSpawn(GameObject spawnObject)
    {
        base.OnSpawn(spawnObject);
        shouldSpawn = false;
        timeSinceSpawn = 0.0f;
        spawnTime = baseSpawnTime + Random.Range(-spawnTimeVariation, spawnTimeVariation);
    }
}
