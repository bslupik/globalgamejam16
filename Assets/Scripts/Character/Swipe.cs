﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swipe : MonoBehaviour {

    int swipableLayerMask;

	// Use this for initialization
	void Start () {
        swipableLayerMask = LayerMask.GetMask(Tags.Layers.swipable);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {

            StartCoroutine(OnClick());
        }
	}

    IEnumerator OnClick()
    {
        Vector2 previousPoint = Draggable.mousePosInWorld();

        HashSet<Transform> alreadyHitObjects = new HashSet<Transform>();

        while (Input.GetMouseButton(0))
        {
            Vector2 newPoint = Draggable.mousePosInWorld();

            RaycastHit2D[] hitObjects = Physics2D.LinecastAll(previousPoint, newPoint, swipableLayerMask);


            foreach (RaycastHit2D hit in hitObjects)
            {
                if (!alreadyHitObjects.Contains(hit.transform))
                {
                    alreadyHitObjects.Add(hit.transform);
                    foreach (Swipable swipe in hit.transform.GetComponentsInChildren<Swipable>())
                    {
                        swipe.Notify(new SwipableMessage());
                    }
                }
            }

            previousPoint = newPoint;

            yield return null;
        }
    }
}

public interface Swipable
{
    void Notify(SwipableMessage m);
}

public class SwipableMessage
{

}

