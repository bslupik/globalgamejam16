using UnityEngine;
using System.Collections;

public class SpawnOnClick : SpawnCondition
{
    public bool shouldSpawn = false;

	public override void Start()
	{
		base.Start();
	}
	
	public override void Update()
	{
		base.Update();
	}

    public void OnMouseDown()
    {
        shouldSpawn = spawnedObject == null;
    }

    public override bool ShouldSpawn()
    {
        return base.ShouldSpawn() && shouldSpawn;
    }

    public override void OnSpawn(GameObject spawnObject)
    {
        base.OnSpawn(spawnObject);
        shouldSpawn = false;
        spawnedObject = spawnObject;
    }
}
