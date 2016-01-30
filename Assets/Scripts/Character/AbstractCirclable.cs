using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class AbstractCirclable : MonoBehaviour, ICirclable
{
    public void Notify(Circle cir)
    {
        Debug.Log("Circled!");
    }

    public void Notify(PolygonCollider2D col) { }
}
