using UnityEngine;
using System.Collections;

public class Choppable : AbstractSwipable
{
    public Sprite chopped;
    public int type;
    public bool choppable = false;
    
	public override void Start()
	{
		base.Start();
        Debug.Log("DING!");
	}
	
	public override void Update()
	{
		base.Update();
	}

    public override void Notify(SwipableMessage m)
    {
        base.Notify(m);
        if (!choppable)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = chopped;
        level.Chopped(type);
    }

    public void animate(bool active)
    {
        choppable = active;
        Vector3 baseScale = transform.localScale;
        Callback.DoLerp((float l) => transform.localScale = l * baseScale, 0.1f, this);
    }
}
