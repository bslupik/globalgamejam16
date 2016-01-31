using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NodeWalker : Base {

    public AnimatedFourDirections animated;
    public float speed = 2.0f;
    protected Queue<MazeNode> nodes;
    protected MazeNode targetNode;
    protected Vector3 direction;
    protected bool initialized;

    public override void Start () {
		base.Start();
        initialized = false;
	}
	
	public override void Update () {
        if(!initialized)
        {
            Reset();
            initialized = true;
        }
		base.Update();

        if(isAtNode() && !isDone())
        {
            moveToNextNode();
        }

        if(!isDone())
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
	}

    public void Reset()
    {
        MazeNode[] nodeArray = GameObject.FindObjectsOfType<MazeNode>();
        Array.Sort(nodeArray, delegate (MazeNode x, MazeNode y)
        {
            return x.order - y.order;
        });
        nodes = new Queue<MazeNode>(nodeArray);
        moveToNextNode();
    }

    protected void moveToNextNode()
    {
        if(nodes.Count == 0)
        {
            targetNode = null;
            return;
        }
        targetNode = nodes.Dequeue();
        direction = (targetNode.transform.position - transform.position).normalized;
        animated.SetDirection(getAnimationDirection());
    }

    protected AnimatedFourDirections.Direction getAnimationDirection()
    {
        if (Mathf.Abs(direction.x - 1.0f) < 0.1f)
            return AnimatedFourDirections.Direction.Right;
        else if (Mathf.Abs(direction.x + 1.0f) < 0.1f)
            return AnimatedFourDirections.Direction.Left;
        else if (Mathf.Abs(direction.y - 1.0f) < 0.1f)
            return AnimatedFourDirections.Direction.Up;

        return AnimatedFourDirections.Direction.Down;
    }

    protected bool isDone()
    {
        return targetNode == null;
    }

    protected bool isAtNode()
    {
        if(isDone())
            return false;
        return Vector2.Distance(this.transform.position, targetNode.transform.position) < 0.1f;
    }
}
