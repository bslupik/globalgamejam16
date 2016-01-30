using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Draggable : Base {

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    protected virtual void OnMouseDown()
    {
        StartCoroutine(DoDrag());
    }

    protected virtual IEnumerator DoDrag()
    {
        Vector3 relativeMousePos = mousePosInWorld() - this.transform.position;

        while (Input.GetMouseButton(0)) //while being dragged
        {
            setPosition(mousePosInWorld() - relativeMousePos);
            yield return null;
        }
        OnDragEnd();
    }

    protected virtual void setPosition(Vector2 position)
    {
        this.transform.position = position;
    }

    protected virtual void OnDragEnd()
    {

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

