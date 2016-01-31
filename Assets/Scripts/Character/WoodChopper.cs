using UnityEngine;
using System.Collections;

public class WoodChopper : Base
{
    public GameObject[] conveyerBelt;
    public float timeSinceLastMove;
    public float moveTime;
    public float moveTimeBase;
    public float moveTimeVariation;
    public GameObject[] choppableObjects;
    public float[] choppableFrequency;
    public Vector3 axis;

	public override void Start()
	{
		base.Start();
        conveyerBelt = new GameObject[3];
        timeSinceLastMove = 0.0f;
    }
	
	public override void Update()
	{
		base.Update();
        timeSinceLastMove += Time.deltaTime;
        if (timeSinceLastMove >= moveTime)
        {
            timeSinceLastMove -= moveTime;
            MoveItems();
        }
    }

    public GameObject NewConveyerItem()
    {
        float totalFrequency = 0.0f;
        for (int i = 0; i < choppableFrequency.Length; ++i)
        {
            totalFrequency += choppableFrequency[i];
        }

        
        float random = Random.Range(0.0f, totalFrequency);
        float position = choppableFrequency[0];
        int index = 0;
        while (position < random)
        {
            index++;
            position += choppableFrequency[index];
        }

        GameObject newItem = (GameObject) GameObject.Instantiate(choppableObjects[index], new Vector3(transform.position.x - 1.0f, transform.position.y + 2.0f, transform.position.z), Quaternion.identity);
        newItem.GetComponent<SpriteRenderer>().enabled = false;
        newItem.transform.parent = transform;
        return newItem;
    }

    public void MoveItems()
    {
        if (conveyerBelt[0] != null)
        {
            conveyerBelt[0].transform.Translate(axis * 1.0f);
            conveyerBelt[0].GetComponent<Choppable>().animate(true);
            conveyerBelt[0].GetComponent<SpriteRenderer>().enabled = true;
        }

        if (conveyerBelt[1] != null)
        {
            conveyerBelt[1].transform.Translate(axis * 1.0f);
            conveyerBelt[1].GetComponent<Choppable>().choppable = false;
            conveyerBelt[1].GetComponent<SpriteRenderer>().enabled = false;
        }

        if (conveyerBelt[2] != null)
        {
            GameObject.Destroy(conveyerBelt[2]);
        }

        conveyerBelt[2] = conveyerBelt[1];
        conveyerBelt[1] = conveyerBelt[0];
        conveyerBelt[0] = NewConveyerItem();

        moveTime = moveTimeBase + Random.Range(-moveTimeVariation, moveTimeVariation);
    }
}
