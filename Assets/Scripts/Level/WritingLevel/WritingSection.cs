using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class WritingSection : Base, ISwipable {
    BoxCollider2D col;
    LineRenderer rend;
    Vector3 start;
    Vector3 end;

	// Use this for initialization
	void Awake () {
	    rend = GetComponent<LineRenderer>();
        col = GetComponent<BoxCollider2D>();
	}

    public void Initialize(Vector2 start, Vector2 end, float width)
    {
        this.start = start;
        this.end = end;
        col.size = new Vector2(Vector2.Distance(start, end), width);
        rend.SetWidth(width, width);
        rend.SetPositions(new Vector3[] { start, end });
        this.transform.position = (start + end) / 2;
        this.transform.rotation = (end - start).ToRotation();
    }

    public void Notify(SwipableMessage m)
    {
        rend.SetColors(Color.white, Color.white);
    }
}
