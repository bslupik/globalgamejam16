using UnityEngine;
using System.Collections;
using System.IO;

public class Savable : MonoBehaviour
{
    public int savableID;
    public float timeToSpawn = 0.0f;
    public float[] metadata;

	public void Start()
	{
        GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>().RegisterSavable(this);
	}

    public virtual void Save(StreamWriter output)
    {
        output.Write(timeToSpawn);
        output.Write(' ');
        output.Write(savableID);
        output.Write(' ');
        output.Write(transform.position.x);
        output.Write(' ');
        output.Write(transform.position.y);
        output.Write(' ');
        output.Write(transform.position.z);
        for (int i = 0; i < metadata.Length; ++i)
        {
            output.Write(' ');
            output.Write(metadata[i]);
        }
    }
}
