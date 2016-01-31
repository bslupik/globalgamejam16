using UnityEngine;
using System.Collections;

public class RectRoamer : Base {

    public RectTransform rect;
    public float speed;

    private Vector2 start;
    private Vector2 dest;
    private float distanceTraveled;
    private float totalDistance;

	public override void Start () {
		base.Start();
        newDestination();
    }
	
	public override void Update () {
		base.Update();
        
        this.distanceTraveled += speed * Time.deltaTime;
        float progress = distanceTraveled / totalDistance;
        this.transform.position = Vector2.Lerp(start, dest, progress);

        if(Mathf.Abs(distanceTraveled - totalDistance) < 0.1f)
        {
            newDestination();
        }
	}

    protected void newDestination()
    {
        start = this.transform.position;
        dest = randomRectPosition();
        distanceTraveled = 0.0f;
        totalDistance = Vector2.Distance(start, dest);
        flipSprite();
    }

    protected void flipSprite()
    {
        // Assuming sprites are by default facing to the right
        SpriteRenderer spriteRenderer = this.transform.GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            spriteRenderer.flipX = (dest.x < transform.position.x);
        }
    }

    protected Vector2 randomRectPosition()
    {
        float halfWidth = rect.sizeDelta.x / 2.0f;
        float halfHeight = rect.sizeDelta.y / 2.0f;
        return new Vector2(
            Random.Range(rect.position.x - halfWidth, rect.position.x + halfWidth),
            Random.Range(rect.position.y - halfHeight, rect.position.y + halfHeight)
        );
    }
}
