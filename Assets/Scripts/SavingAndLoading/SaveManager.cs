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
    public Vector3 spawnPosition;
    public List<float> metadata;

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

public class SaveManager : MonoBehaviour
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

	public void Start()
	{
        objectsToSpawn = new List<SpawnData>();
        loadedObjects = new List<GameObject>();

        if (fileName != "")
        {
            Load();
        }
    }
	
	public void Update()
	{
        gameTime += Time.deltaTime;
        //print(objectsToSpawn.Count);
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
                if (i != savableObjects.Count - 1)
                {
                    output.Write('\n');
                }
            }
        }

        output.Close();
    }

    public void UnloadLevel()
    {
        for (int i = 0; i < loadedObjects.Count; ++i)
        {
            if (loadedObjects[i] != null)
            {
                GameObject.Destroy(loadedObjects[i]);
            }
        }
        loadedObjects = new List<GameObject>();
    }

    public void Load()
    {
        Load(fileName);
    }

    public void Load(string fileToLoad)
    {
        TextAsset inputFile = (TextAsset)Resources.Load(inputFilePath + fileToLoad);
        if (inputFile == null)
        {
            return;
        }

        UnloadLevel();

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
        newObject.spawnPosition = new Vector3(float.Parse(data[2]), float.Parse(data[3]), float.Parse(data[4]));
        switch (newObject.savableID)
        {
            case 27: // Maze Node
            case 9: // Acupuncture level manager
                newObject.metadata = new List<float>();
                if (data.Length >= 6)
                {
                    newObject.metadata.Add(float.Parse(data[5]));
                }
                else
                {
                    newObject.metadata.Add(0);
                }
                break;
            case 2: // Torch
                newObject.metadata = new List<float>();
                if (data.Length >= 7)
                {
                    newObject.metadata.Add(float.Parse(data[5]));
                    newObject.metadata.Add(float.Parse(data[6]));
                }
                else
                {
                    newObject.metadata.Add(0);
                    newObject.metadata.Add(0);
                }
                break;
            case 0: // Villager spawner
            case 1: // Grave
            case 6: // Super grave
            case 12: // Fire hut
            case 23: // Dodo spawner
            case 25: // Lasso animal
                newObject.metadata = new List<float>();
                if (data.Length >= 8)
                {
                    newObject.metadata.Add(float.Parse(data[5]));
                    newObject.metadata.Add(float.Parse(data[6]));
                    newObject.metadata.Add(float.Parse(data[7]));
                }
                else
                {
                    newObject.metadata.Add(0);
                    newObject.metadata.Add(0);
                    newObject.metadata.Add(0);
                }
                break;
            case 10: //puncture point
                newObject.metadata = new List<float>();
                newObject.metadata.Add(float.Parse(data[5]));
                break;
            
        }
        objectsToSpawn.Add(newObject);
    }

    public void SpawnObject(SpawnData data)
    {
        GameObject instantiatedObject = (GameObject) GameObject.Instantiate(spawnableObjects[data.savableID], data.spawnPosition, Quaternion.identity);
        loadedObjects.Add(instantiatedObject);
        switch (data.savableID)
        {
            case 27: // Maze Node
                instantiatedObject.GetComponentInChildren<MazeNode>().setOrder = (int) data.metadata[0];
                break;
            case 9: // Acupuncture level manager
                instantiatedObject.GetComponentInChildren<Level>().orderedNumbers = new int[(int) data.metadata[0]];
                for (int i = 0; i < data.metadata[0]; ++i)
                {
                    instantiatedObject.GetComponentInChildren<Level>().orderedNumbers[i] = i;
                }
                break;
            case 2: // Torch
                if (data.metadata[0] != 0)
                {
                    instantiatedObject.GetComponentInChildren<FlameSpawner>().flameLifeBase = data.metadata[0];
                }
                if (data.metadata[1] != 0)
                {
                    instantiatedObject.GetComponentInChildren<FlameSpawner>().flameLifeVariation = data.metadata[1];
                }
                break;
            case 0: // Villager spawner
            case 1: // Grave
            case 6: // Super grave
            case 12: // Fire hut
            case 23: // Dodo spawner
            case 25: // Lasso animal
                if (data.metadata[0] != 0)
                {
                    instantiatedObject.GetComponentInChildren<TimedSpawnCondition>().spawnTime = data.metadata[0];
                }
                if (data.metadata[1] != 0)
                {
                    instantiatedObject.GetComponentInChildren<TimedSpawnCondition>().baseSpawnTime = data.metadata[1];
                }
                if (data.metadata[2] != 0)
                {
                    instantiatedObject.GetComponentInChildren<TimedSpawnCondition>().spawnTimeVariation = data.metadata[2];
                }
                break;
            case 10: //puncture point
                instantiatedObject.GetComponent<PuncturePoint>().setOrder = (int)(data.metadata[0]);
                break;
        }
    }

    public void UpdateFileName()
    {
        if (GameObject.FindWithTag(Tags.fileNameInput) != null)
        {
            fileName = GameObject.FindWithTag(Tags.fileNameInput).GetComponent<InputField>().text;
        }
    }
}
