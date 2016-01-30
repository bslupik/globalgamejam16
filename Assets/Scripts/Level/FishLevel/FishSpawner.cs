using UnityEngine;
using System.Collections;

public class FishSpawner : MonoBehaviour {

    [SerializeField]
    protected GameObject fishPrefab;

    [SerializeField]
    protected float spawnCooldown;

    [SerializeField]
    protected float spawnRange;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnFish());
	}

    IEnumerator SpawnFish()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(spawnCooldown);
            GameObject fish = Instantiate(fishPrefab);
            fish.transform.Find("FishPhysics").position = spawnRange * Random.insideUnitCircle;
            fish.transform.Find("FishVisuals").position = new Vector2(0, -100f);
        }
    }
}
