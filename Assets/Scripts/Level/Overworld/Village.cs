using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Village : MonoBehaviour {

    SaveManager saves;
    SceneChangeScripting scripting;

    [SerializeField]
    protected string[] levelNames;

    static bool active = true;

	void Start () {
        saves = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        scripting = Camera.main.GetComponent<SceneChangeScripting>();
	}

    void OnMouseDown()
    {
        if (active)
        {
            active = false;
            StartCoroutine(Spawning());
        }
    }

    IEnumerator Spawning()
    {
        yield return scripting.FadeOut();
        Time.timeScale = 0;
        saves.Load(levelNames[Random.Range(0, levelNames.Length - 1)]);
        yield return scripting.FadeIn();
        Time.timeScale = 1;
        yield return new WaitForSeconds(30f);
        saves.UnloadLevel();
        active = true;
    }
}
