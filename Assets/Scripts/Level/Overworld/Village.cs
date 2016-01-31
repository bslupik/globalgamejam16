using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Village : MonoBehaviour {

    public static Village activeVillage;

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
        activeVillage = this;
        yield return scripting.FadeOut();
        Time.timeScale = 0;
        saves.Load(levelNames[Random.Range(0, levelNames.Length - 1)]);
        yield return scripting.FadeIn();
        Debug.Log("Timescale");
        Time.timeScale = 1f;
        yield return new WaitForSeconds(30f);
        saves.UnloadLevel();
        active = true;
        Debug.Log("Back to overworld");
        activeVillage = null;
    }

    public void Unload()
    {
        saves.UnloadLevel();
        active = true;
        Debug.Log("Back to overworld");
        activeVillage = null;
        StopAllCoroutines();
    }
}
