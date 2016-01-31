using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class AbstractSwipable : Base, ISwipable {
    public virtual void Notify(SwipableMessage m)
    {
        level.PlayerActedBuffer();
    }
}
