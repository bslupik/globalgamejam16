using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class HerdableAnimal : Base, ICirclable {

    public void Notify(Circle cir)
    {
        level.PlayerActed();
        Destroy(this.gameObject);
    }

    public void Notify(PolygonCollider2D col) { }
}
