using UnityEngine;
using System.Collections;

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

        while (Input.GetMouseButton(0))
        {
            Vector2 newPoint = Draggable.mousePosInWorld();

            RaycastHit2D[] hitObjects = Physics2D.LinecastAll(previousPoint, newPoint, swipableLayerMask);
            foreach (RaycastHit2D hit in hitObjects)
            {

                // TODO : filter out multiple hits for slow swipes

                foreach (Swipable swipe in hit.transform.GetComponentsInChildren<Swipable>())
                {
                    swipe.Notify(new SwipableMessage());
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

