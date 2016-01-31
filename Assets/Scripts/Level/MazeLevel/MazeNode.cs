using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class MazeNode : OnBeatDraggable, IResettable
{
    public int order { get; set; }

    [SerializeField]
    public int setOrder = -1;

    LineRenderer rend;

    PlayerNodeWalker player;

    bool locked = false;

    void Awake()
    {
        rend = GetComponent<LineRenderer>();
    }

    public override void Start()
    {
        base.Start();
        if (setOrder != -1)
        {
            order = setOrder;
        }
        player = GameObject.FindObjectOfType<PlayerNodeWalker>();
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
        rend.enabled = false;
        if(player.IsAtNode())
        {
            base.OnDragEnd();
            if (!(level.PlayerActed(this)))
            {
                reset();
            }
            else
            {
                locked = true;
                player.PlayerMoved();
            }
        }
    }

    public void reset()
    {
        locked = false;
        gameObject.SetActive(true);
        rend.SetPosition(1, Vector3.zero);
        rend.enabled = true;
    }
}
