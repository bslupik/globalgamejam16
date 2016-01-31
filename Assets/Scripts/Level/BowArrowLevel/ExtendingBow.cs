using UnityEngine;
using System.Collections;

public class ExtendingBow : OnBeatDraggable {

    public float distanceToFire;
    public LineRenderer leftString;
    public LineRenderer rightString;
    public Transform bowArrow;
    private BowReleasedSpawnCondition arrowSpawnCondition;
    private volatile float mouseDist;

	public override void Start () {
		base.Start();
        leftString = transform.Find("LeftString").GetComponent<LineRenderer>();
        rightString = transform.Find("RightString").GetComponent<LineRenderer>();
        bowArrow = transform.Find("BowArrow");
        Spawner arrowSpawner = GameObject.FindObjectOfType<ArrowSpawner>().GetComponent<Spawner>();
        arrowSpawnCondition = arrowSpawner.GetComponent<BowReleasedSpawnCondition>();
        mouseDist = 0.0f;
	}
	
	public override void Update () {
		base.Update();
        leftString.SetPosition(1, new Vector3(-mouseDist, -0.03f, 0.0f));
        rightString.SetPosition(1, new Vector3(-mouseDist, 0.03f, 0.0f));
        bowArrow.transform.localPosition = new Vector3(0.4f - mouseDist, 0.0f, 0.0f);
    }

    protected override IEnumerator DoDrag()
    {
        bowArrow.GetComponent<SpriteRenderer>().enabled = true;
        while (Input.GetMouseButton(0)) //while being dragged
        {
            Vector3 direction = (transform.position - mousePosInWorld()).normalized;
            transform.rotation = ((Vector2)direction).ToRotation(Vector3.forward);

            mouseDist = Mathf.Min(distanceToFire, Vector3.Distance(mousePosInWorld(), transform.position));

            yield return null;
        }
    }

    protected override void OnMouseUp()
    {
        if (mouseDist >= distanceToFire)
        {
            Vector3 direction = (transform.position - mousePosInWorld()).normalized;
            arrowSpawnCondition.ReleaseBow(this.transform.position, direction);
            // SOUND DO: Bow Shot
            // sound.PlaySound(2);
        }
        bowArrow.GetComponent<SpriteRenderer>().enabled = false;
        mouseDist = 0.0f;
    }

}
