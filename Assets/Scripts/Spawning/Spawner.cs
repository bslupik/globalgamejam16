using UnityEngine;
using System.Collections;

public class Spawner : Base
{
    public SpawnCondition condition;
    public GameObject spawnObject;

	public override void Start()
	{
		base.Start();
        condition = GetComponent<SpawnCondition>();
    }
	
	public override void Update()
	{
		base.Update();
        if (condition.ShouldSpawn())
        {
            condition.OnSpawn(Spawn());
        }
	}
    
    public virtual GameObject Spawn()
    {
        GameObject spawnedObject = (GameObject) GameObject.Instantiate(spawnObject, transform.position, transform.rotation);
        spawnedObject.transform.parent = transform;
        return spawnedObject;
    }
}
