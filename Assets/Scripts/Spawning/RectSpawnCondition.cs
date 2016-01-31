using UnityEngine;
using System.Collections;

public class RectSpawnCondition : NTimedSpawnCondition {

    public RectTransform[] spawnZones;
    public RectTransform moveRect;

	public override void Start () {
		base.Start();
	}
	
	public override void Update () {
		base.Update();
	}

    public override void OnSpawn(GameObject spawnObject)
    {
        base.OnSpawn(spawnObject);
        RectRoamer rectRoamer = spawnObject.GetComponent<RectRoamer>();
        rectRoamer.rect = moveRect;
        rectRoamer.transform.position = randomRectPosition();
    }

    protected Vector2 randomRectPosition()
    {
        int zoneIndex = Random.Range(0, spawnZones.Length);
        RectTransform rect = spawnZones[zoneIndex];
        float halfWidth = rect.sizeDelta.x / 2.0f;
        float halfHeight = rect.sizeDelta.y / 2.0f;
        return new Vector2(
            Random.Range(rect.position.x - halfWidth, rect.position.x + halfWidth),
            Random.Range(rect.position.y - halfHeight, rect.position.y + halfHeight)
        );
    }
}
