using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour
{
    public SoundManager sound;
    public Level level;

    public virtual void Start()
    {
        sound = GameObject.FindWithTag(Tags.soundManager).GetComponent<SoundManager>();
        level = GameObject.FindWithTag(Tags.levelManager).GetComponent<Level>();
    }

    public virtual void Update()
	{
	
	}
}
