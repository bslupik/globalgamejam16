using UnityEngine;
using System.Collections;
using System.IO;

public class Savable : MonoBehaviour
{
    public int savableID;
    public float timeToSpawn = 0.0f;

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
        switch (savableID)
        {
            case 10: //acupuncture points
                output.Write(' ');
                output.Write(GetComponent<PuncturePoint>().order);
                break;
        }
    }
}
