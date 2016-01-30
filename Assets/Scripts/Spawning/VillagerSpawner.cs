using UnityEngine;
using System.Collections;

public class VillagerSpawner : Spawner
{
    public float speedBase;
    public float speedVariation;

	public override void Start()
	{
		base.Start();
	}
	
	public override void Update()
	{
		base.Update();
	}

    public override GameObject Spawn()
    {
        GameObject spawnedObject = base.Spawn();
        spawnedObject.GetComponent<Drift>().speed = speedBase + Random.Range(-speedVariation, speedVariation);
        return spawnedObject;
    }
}
