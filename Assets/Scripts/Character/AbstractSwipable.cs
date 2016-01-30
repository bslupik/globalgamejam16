using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class AbstractSwipable : MonoBehaviour, Swipable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Notify(SwipableMessage m)
    {
        Debug.Log("HIT!");
    }
}
