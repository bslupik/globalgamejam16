using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

public class SpawnData : IComparable<SpawnData>
{
    public float spawnTime;
    public int savableID;
    public float spawnX;
    public float spawnY;

    public int CompareTo(SpawnData other)
    {
        if (other.spawnTime < spawnTime)
        {
            return 1;
        }
        else if (spawnTime < other.spawnTime)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}

public class SaveManager : Base
{
    public string outputFilePath;
    public string inputFilePath;
    public string fileName;
    public List<Savable> savableObjects;
    public GameObject[] spawnableObjects;
    public List<SpawnData> objectsToSpawn;
    public float gameTime;
    public List<GameObject> loadedObjects;

    public void Awake()
    {
        savableObjects = new List<Savable>();
    }

	public override void Start()
	{
		base.Start();
        objectsToSpawn = new List<SpawnData>();
        loadedObjects = new List<GameObject>();
    }
	
	public override void Update()
	{
		base.Update();
        gameTime += Time.deltaTime;

        while (objectsToSpawn.Count > 0 && gameTime >= objectsToSpawn[objectsToSpawn.Count - 1].spawnTime)
        {
            SpawnObject(objectsToSpawn[objectsToSpawn.Count - 1]);
            objectsToSpawn.RemoveAt(objectsToSpawn.Count - 1);
        }
    }

    public void RegisterSavable(Savable saveObject)
    {
        savableObjects.Add(saveObject);
    }

    public void Save()
    {
        StreamWriter output = new StreamWriter(outputFilePath + fileName + ".txt");
        for (int i = 0; i < savableObjects.Count; ++i)
        {
            if (savableObjects[i] != null)
            {
                savableObjects[i].Save(output);
            }
        }

        output.Close();
    }

    public void Load()
    {
        print(inputFilePath + fileName + ".txt");
        TextAsset inputFile = (TextAsset)Resources.Load(inputFilePath + fileName);
        if (inputFile == null)
        {
            //return;
        }

        for (int i = 0; i < loadedObjects.Count; ++i)
        {
            if (loadedObjects[i] != null)
            {
                GameObject.Destroy(loadedObjects[i]);
            }
        }
        loadedObjects = new List<GameObject>();

        string[] lines = inputFile.text.Split('\n');
        for (int i = 0; i < lines.Length; ++i)
        {
            if (lines[i] != "")
            {
                LoadNextObject(lines[i]);
            }
        }
        objectsToSpawn.Sort();
        objectsToSpawn.Reverse();
        gameTime = 0.0f;
    }

    public void LoadNextObject(string line)
    {
        string[] data = line.Split(' ');
        SpawnData newObject = new SpawnData();
        newObject.spawnTime = float.Parse(data[0]);
        newObject.savableID = int.Parse(data[1]);
        newObject.spawnX = float.Parse(data[2]);
        newObject.spawnY = float.Parse(data[3]);
        objectsToSpawn.Add(newObject);
    }

    public void SpawnObject(SpawnData data)
    {
        loadedObjects.Add((GameObject) GameObject.Instantiate(spawnableObjects[data.savableID], new Vector3(data.spawnX, data.spawnY), Quaternion.identity));
    }

    public void UpdateFileName()
    {
        fileName = GameObject.FindWithTag("FileNameInput").GetComponent<InputField>().text;
    }
}
