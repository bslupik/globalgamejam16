using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public SpawnCondition condition;
    public GameObject spawnObject;

	public virtual void Start()
	{
        condition = GetComponent<SpawnCondition>();
    }

    public virtual void Update()
	{
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
