using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class MazeNode : OnBeatDraggable, IResettable
{
    public int order { get; set; }

    [SerializeField]
    protected int setOrder = -1;

    LineRenderer rend;

    bool locked = false;

    void Awake()
    {
        rend = GetComponent<LineRenderer>();
        if (setOrder != -1)
        {
            order = setOrder;
        }
    }

    protected override void OnMouseDown()
    {
        if (!locked)
        {
            base.OnMouseDown();
        }
    }

    protected override void setPosition(Vector2 position)
    {
        rend.SetPosition(1, transform.InverseTransformPoint(position));
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
        if (!(level.PlayerActed(this)))
        {
            reset();
        }
        else
        {
            locked = true;
        }
    }

    public void reset()
    {
        locked = false;
        gameObject.SetActive(true);
        rend.SetPosition(1, Vector3.zero);
    }
}
