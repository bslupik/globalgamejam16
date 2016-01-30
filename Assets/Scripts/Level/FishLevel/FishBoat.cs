using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class FishBoat : Base {

    [SerializeField]
    protected GameObject boatVisuals;

    [SerializeField]
    protected float timeBetweenNets;

    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }
	public override void Start () {
        base.Start();
        StartCoroutine(DoFishing());
	}

    IEnumerator DoFishing()
    {
        for (; ; )
        {
            Vector2 boatPos = boatVisuals.transform.position;
            Vector2 netPos = new Vector2(Random.Range(-8f, 8f), Random.Range(-5f, 3f));
            yield return Callback.DoLerp((float l) => boatVisuals.transform.position = Vector2.Lerp(boatPos, new Vector2(netPos.x, boatPos.y), l), 1, this);
            boatPos = boatVisuals.transform.position;
            this.transform.position = boatPos;
            col.enabled = true;
            yield return Callback.DoLerp((float l) => this.transform.position = Vector2.Lerp(boatPos, netPos, l), 3, this);
            yield return Callback.DoLerp((float l) => this.transform.position = Vector2.Lerp(boatPos, netPos, l), 3, this, reverse : true);
            yield return new WaitForSeconds(timeBetweenNets);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Fish otherFish = other.transform.GetComponent<Fish>();
        if (otherFish)
            otherFish.Despawn();
        
    }
}
