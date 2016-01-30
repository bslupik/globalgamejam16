using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SaveManager : Base
{
    public string filePath;
    public string outputFileName;
    public List<Savable> savableObjects;

	public override void Start()
	{
		base.Start();
	}
	
	public override void Update()
	{
		base.Update();
	}

    public void RegisterSavable(Savable saveObject)
    {
        savableObjects.Add(saveObject);
    }

    public void Save()
    {
        StreamWriter output = new StreamWriter(filePath + outputFileName);
        for (int i = 0; i < savableObjects.Count; ++i)
        {
            if (savableObjects[i] != null)
            {
                savableObjects[i].Save(output);
            }
        }

        output.Close();
    }
}
