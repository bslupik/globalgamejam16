using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour {

    void OnMouseDown()
    {

        StartCoroutine(DoDrag());
    }

    IEnumerator DoDrag()
    {
        Vector3 relativeMousePos = mousePosInWorld() - this.transform.position;

        while (Input.GetMouseButton(0)) //while being dragged
        {
            this.transform.position = mousePosInWorld() - relativeMousePos;
            yield return null;
        }
    }

    public static Vector3 mousePosInWorld()
    {
        return mousePosInWorld(Camera.main.transform); //I don't want to use default parameters, because that would involve some extra computation through null-coalescing (the ?? thing)
    }

    public static Vector3 mousePosInWorld(Transform cameraTransform)
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = -cameraTransform.position.z;
        return screenPoint.toWorldPoint();
    }
}

