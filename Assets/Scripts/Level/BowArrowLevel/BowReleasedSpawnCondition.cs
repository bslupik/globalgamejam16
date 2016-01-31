using UnityEngine;
using System.Collections;

public class BowReleasedSpawnCondition : NSpawnCondition {

    private bool bowReleased;
    private Vector2 position;
    private Vector2 direction;

	public override void Start () {
		base.Start();
	}
	
	public override void Update () {
		base.Update();
	}

    public void ReleaseBow(Vector2 position, Vector2 direction)
    {
        this.bowReleased = true;
        this.position = position;
        this.direction = direction;
    }

    public override bool ShouldSpawn()
    {
        if (bowReleased)
        {
            bowReleased = false;
            return base.ShouldSpawn();
        }
        return false;
    }

    public override void OnSpawn(GameObject spawnObject)
    {
        base.OnSpawn(spawnObject);
        spawnObject.transform.position = position;
        spawnObject.transform.rotation = direction.ToRotation(Vector3.forward);
        Drift drift = spawnObject.GetComponent<Drift>();
        drift.direction = direction;
    }
}
