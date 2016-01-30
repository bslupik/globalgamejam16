using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Fish : Base, ICirclable {

    Collider2D col;
    Rigidbody2D rigid;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Notify(Circle cir)
    {
        Physics2D.IgnoreCollision(col, cir.Collider, true);
        rigid.position = cir.averageCenter + Random.insideUnitCircle * 0.1f;
    }

    public void Notify(PolygonCollider2D otherCol)
    {
        Physics2D.IgnoreCollision(col, otherCol, false);
    }
}
