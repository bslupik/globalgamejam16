using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PuncturePoint : Base {

    public int order { get; set; }

    [SerializeField]
    protected int setOrder = -1;

    void Awake()
    {
        if (setOrder != -1)
        {
            order = setOrder;
        }
    }

    public void OnMouseDown()
    {
        if ((level as PunctureLevel).PlayerActed(order))
        {
            Debug.Log("puncture");
            Destroy(this.gameObject);
        }
    }
}
