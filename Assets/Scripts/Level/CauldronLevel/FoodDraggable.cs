using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FoodDraggable : OnBeatDraggable {

    public int order { get; set; }

    [SerializeField]
    protected int setOrder = -1;

    Vector2 originalLocation;

    Collider2D col;
    public Collider2D Collider { get { return col; } }

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        originalLocation = transform.position;
        if (setOrder != -1)
        {
            order = setOrder;
        }
    }

    public void reset()
    {
        this.gameObject.SetActive(true);
        rigid.position = originalLocation;
    }

    protected override void setPosition(Vector2 position)
    {
        rigid.position = position;
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
        if((level as CauldronLevel).PlayerActed(this))
            this.gameObject.SetActive(false);
    }
}
