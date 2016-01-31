using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swipe : Base {

    int swipableLayerMask;

    [SerializeField]
    protected float minDistance;

	// Use this for initialization
	public override void Start () {
        base.Start();
        swipableLayerMask = LayerMask.GetMask(Tags.Layers.swipable, Tags.Layers.overworld);
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {

            StartCoroutine(OnClick());
        }
	}

    IEnumerator OnClick()
    {
        Vector2 startPoint = Draggable.mousePosInWorld();
        Vector2 previousPoint = startPoint;

        HashSet<Transform> alreadyHitObjects = new HashSet<Transform>();

        while (Input.GetMouseButton(0))
        {
            Vector2 newPoint = Draggable.mousePosInWorld();
            if (Vector2.Distance(startPoint, newPoint) < minDistance)
                yield return null;
            else
                break;
        }

        while (Input.GetMouseButton(0))
        {
            Vector2 newPoint = Draggable.mousePosInWorld();

            RaycastHit2D[] hitObjects = Physics2D.LinecastAll(previousPoint, newPoint, swipableLayerMask);


            foreach (RaycastHit2D hit in hitObjects)
            {
                if (!alreadyHitObjects.Contains(hit.transform))
                {
                    alreadyHitObjects.Add(hit.transform);
                    foreach (ISwipable swipe in hit.transform.GetComponentsInChildren<ISwipable>())
                    {
                        swipe.Notify(new SwipableMessage());
                    }
                }
            }

            previousPoint = newPoint;

            yield return null;
        }

        level.FlushScoreBuffer();
    }
}

public interface ISwipable
{
    void Notify(SwipableMessage m);
}

public class SwipableMessage
{

}

