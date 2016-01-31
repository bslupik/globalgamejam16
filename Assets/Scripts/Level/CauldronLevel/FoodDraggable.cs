using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FoodDraggable : OnBeatDraggable, IResettable, IObserver<int> {
    public int order { get; set; }

    [SerializeField]
    public int setOrder = -1;

    [SerializeField]
    protected Sprite[] sprites;

    Vector2 originalLocation;

    Collider2D col;
    public Collider2D Collider { get { return col; } }

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        originalLocation = transform.position;
    }

    public override void Start()
    {
        base.Start();
        level.Subscribe(this);
        if (setOrder != -1)
        {
            order = setOrder;
        }
        if (order > 2)
            this.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = sprites[order % sprites.Length];
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
        if (level.PlayerActed(this))
        {
            level.Unsubscribe(this);
            Destroy(this.gameObject);
        }
    }

    public void Notify(int i)
    {
        if (order <= i)
        {
            this.gameObject.SetActive(true);
        }
    }
}
