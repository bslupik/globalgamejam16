using UnityEngine;
using System.Collections;

public class ExtendingBow : OnBeatDraggable {

    public const float ROTATION_SPEED = 1.0f;
    public const float MAX_CHARGE = 0.6f;
    public const float DISTANCE_TO_FIRE = 1.0f;
    public LineRenderer leftString;
    public LineRenderer rightString;
    public Transform bowArrow;
    public Spawner arrowSpawner;
    public BowReleasedSpawnCondition arrowSpawnCondition;
    private volatile float bowCharge;

	public override void Start () {
		base.Start();
        leftString = transform.Find("LeftString").GetComponent<LineRenderer>();
        rightString = transform.Find("RightString").GetComponent<LineRenderer>();
        bowArrow = transform.Find("BowArrow");
        arrowSpawnCondition = arrowSpawner.GetComponent<BowReleasedSpawnCondition>();
        bowCharge = 0.0f;
	}
	
	public override void Update () {
		base.Update();
        leftString.SetPosition(1, new Vector3(-bowCharge, -0.03f, 0.0f));
        rightString.SetPosition(1, new Vector3(-bowCharge, 0.03f, 0.0f));
        bowArrow.transform.localPosition = new Vector3(0.4f - bowCharge, 0.0f, 0.0f);
    }

    protected override IEnumerator DoDrag()
    {
        bowArrow.GetComponent<SpriteRenderer>().enabled = true;
        while (Input.GetMouseButton(0)) //while being dragged
        {
            Vector3 direction = (transform.position - mousePosInWorld()).normalized;
            transform.rotation = ((Vector2)direction).ToRotation(Vector3.forward);

            float mouseDist = Vector3.Distance(mousePosInWorld(), transform.position);
            bowCharge = Mathf.Min(MAX_CHARGE, (mouseDist / DISTANCE_TO_FIRE) * MAX_CHARGE);

            yield return null;
        }
    }

    protected override void OnMouseUp()
    {
        if (bowCharge >= MAX_CHARGE)
        {
            Vector3 direction = (transform.position - mousePosInWorld()).normalized;
            arrowSpawnCondition.ReleaseBow(this.transform.position, direction);
            base.sound.PlaySound(0);
        }
        bowArrow.GetComponent<SpriteRenderer>().enabled = false;
        bowCharge = 0.0f;
    }

}
