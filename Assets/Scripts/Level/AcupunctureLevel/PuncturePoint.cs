using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PuncturePoint : Base {

    public int order { get; set; }

    [SerializeField]
    public int setOrder = -1;

    [SerializeField]
    protected Sprite puncturedSprite;

    public override void Start()
    {
        base.Start();
        if (setOrder != -1)
        {
            order = setOrder;
        }
    }

    public void OnMouseDown()
    {
        if (level.PlayerActed(order))
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = puncturedSprite;
        }
    }
}
