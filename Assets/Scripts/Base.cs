using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour
{
    public SoundManager sound;
    public Level level;

    public virtual void Start()
    {
        sound = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        level = GameObject.FindWithTag("LevelManager").GetComponent<Level>();
    }

    public virtual void Update()
	{
	
	}
}
