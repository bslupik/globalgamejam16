using UnityEngine;
using System.Collections;

public class JiggleSpawner : Spawner
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float distanceVariation;

	public override void Start()
	{
		base.Start();
        if (GetComponentInParent<JiggleSpawner>())
        {
            JiggleSpawner parentSpawner = GetComponentInParent<JiggleSpawner>();
            minX = parentSpawner.minX;
            maxX = parentSpawner.maxX;
            minY = parentSpawner.minY;
            maxY = parentSpawner.maxY;
            distanceVariation = parentSpawner.distanceVariation / 4;
        }
        else
        {
            minX = transform.position.x - distanceVariation;
            maxX = transform.position.x + distanceVariation;
            minY = transform.position.y - distanceVariation;
            maxY = transform.position.y + distanceVariation;
        }
	}
	
	public override void Update()
	{
		base.Update();
	}

    public override GameObject Spawn()
    {
        GameObject instantiatedObject = base.Spawn();
        Vector3 variation = new Vector3(Random.Range(-distanceVariation, distanceVariation), Random.Range(-distanceVariation, distanceVariation), 0);
        instantiatedObject.transform.position += variation;
        Vector3 finalPosition = instantiatedObject.transform.position;
        if (finalPosition.x < minX)
        {
            finalPosition.x = minX;
        }
        else if (finalPosition.x > maxX)
        {
            finalPosition.x = maxX;
        }

        if (finalPosition.y < minY)
        {
            finalPosition.y = minY;
        }
        else if (finalPosition.y < maxY)
        {
            finalPosition.y = maxY;
        }
        TimedSpawnCondition instantiatedSpawner = instantiatedObject.GetComponent<TimedSpawnCondition>();
        if (instantiatedSpawner != null)
        {
            instantiatedSpawner.spawnTime = instantiatedSpawner.baseSpawnTime;
            instantiatedSpawner.shouldSpawn = false;
        }
        return instantiatedObject;
    }
}
