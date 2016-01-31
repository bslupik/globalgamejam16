using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class Village : MonoBehaviour {

    public static Village activeVillage;

    WorldManager world;
    SceneChangeScripting scripting;

    public List<int> easy;
    public List<int> medium;
    public List<int> hard;

    static bool active = true;

	void Start () {
        world = GameObject.Find("WorldManager").GetComponent<WorldManager>();
        scripting = Camera.main.GetComponent<SceneChangeScripting>();
	}

    void OnMouseDown()
    {
        if (active && !world.levelFinish.activeSelf)
        {
            active = false;
            Spawning();
        }
    }

    void Spawning()
    {
        activeVillage = this;
        if (world.levelDifficulty == 0) {
            world.StartLevel(easy[Random.Range(0, easy.Count)]);
        } else if (world.levelDifficulty == 1) {
            world.StartLevel(medium[Random.Range(0, medium.Count)]);
        } else {// if (world.levelDifficulty == 2) {
            world.StartLevel(hard[Random.Range(0, hard.Count)]);
        }
    }

    public void OnStart()
    {
        active = false;
    }

    public void OnUnload()
    {
        active = true;
        StopAllCoroutines();
    }
}
