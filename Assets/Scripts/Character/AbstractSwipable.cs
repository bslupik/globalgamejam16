using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class AbstractSwipable : MonoBehaviour, Swipable {

	// Use this for initialization
	public virtual void Start() {
	
	}
	
	// Update is called once per frame
	public virtual void Update() {
	
	}

    public virtual void Notify(SwipableMessage m)
    {
        Debug.Log("HIT!");
    }
}
