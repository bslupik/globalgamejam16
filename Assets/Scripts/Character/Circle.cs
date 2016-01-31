using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Circle : MonoBehaviour {

    [SerializeField]
    protected float maxLength = 8.0f;

    PolygonCollider2D col;

    public PolygonCollider2D Collider { get { return col; } }

    public Vector2 averageCenter { get; set; }

    LineRenderer rend;

    int framesLeft = -1;
    HashSet<ICirclable> circledObjects = new HashSet<ICirclable>();
    void Awake()
    {
        col = GetComponent<PolygonCollider2D>();
        rend = GetComponent<LineRenderer>();
        if (maxLength != 8.0f)
        {
            maxLength = 8.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(OnClick());
        }
    }

    void FixedUpdate()
    {
        if (framesLeft >= 0)
        {
            framesLeft--;
            if (framesLeft < 0)
            {
                col.enabled = false;
                rend.enabled = false;
                foreach (ICirclable circledObject in circledObjects)
                {
                    circledObject.Notify(col);
                }
                circledObjects.Clear();
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        ICirclable circled = other.transform.GetComponent<ICirclable>();
        if (circled != null)
        {
            circledObjects.Add(circled);
            circled.Notify(this);
        }
    }

    IEnumerator OnClick()
    {
        List<Vector3> points = new List<Vector3>();

        rend.enabled = true;

        float length = 0;
        Vector2 sumPoints = Vector2.zero;

        while (Input.GetMouseButton(0) && length < maxLength)
        {
            Vector3 newPoint = Draggable.mousePosInWorld();
            newPoint.z = -8.0f;
            if (points.Count > 0)
            {
                float segmentLength = Vector2.Distance(points[points.Count - 1], newPoint);
                sumPoints += (Vector2)(points[points.Count - 1] + newPoint) * segmentLength / 2;
                length += segmentLength;
            }

            points.Add(newPoint);
            rend.SetVertexCount(points.Count);
            rend.SetPositions(points.ToArray());


            yield return null;
        }

        averageCenter = sumPoints / length;

        col.enabled = true;
        col.points = ListToVector2(points);
        framesLeft = 60;
    }

    Vector2[] ListToVector2(List<Vector3> vectors)
    {
        Vector2[] result = new Vector2[vectors.Count];
        for (int i = 0; i < vectors.Count; i++)
        {
            result[i] = vectors[i];
        }
        return result;
    }
}

public interface ICirclable
{
    void Notify(Circle self);
    void Notify(PolygonCollider2D other);
}