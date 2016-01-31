using UnityEngine;
using System.Collections;

public class AnimatedFourDirections : Base {

    public enum Direction { Left, Right, Up, Down };

    public SpriteRenderer spriteRenderer;
    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;
    public Direction dir = Direction.Down;
    int spriteIndex;

    public override void Start () {
		base.Start();
        if(upSprites.Length != downSprites.Length
            || downSprites.Length != leftSprites.Length
            || leftSprites.Length != rightSprites.Length)
        {
            Debug.Log("One of the arrays of sprites does not have the same length as the rest!");
        }
        spriteIndex = 0;
        if (spriteRenderer != null)
            spriteRenderer.sprite = GetSprite();
    }
	
	public override void Update () {
		base.Update();

        int nextSpriteIndex = (int)(base.level.timeSinceBeat / timePerSprite());
        if (nextSpriteIndex != spriteIndex)
            loadNextSprite();
    }

    public void SetDirection(Direction dir)
    {
        this.dir = dir;
        spriteRenderer.sprite = GetSprite();
    }

    private float timePerSprite()
    {
        if (downSprites.Length == 0)
        {
            return 0.0f;
        }
        return (float)(base.level.timePerBeat / downSprites.Length);
    }

    private void loadNextSprite()
    {
        if (downSprites.Length == 0)
        {
            spriteIndex = -1;
            return;
        }

        spriteIndex = (spriteIndex + 1) % downSprites.Length;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = GetSprite();
        }
    }

    public Sprite GetSprite()
    {
        if (spriteIndex == -1)
            return null;

        if(dir == Direction.Down)
            return downSprites[spriteIndex];
        else if(dir == Direction.Up)
            return upSprites[spriteIndex];
        else if(dir == Direction.Left)
            return leftSprites[spriteIndex];
        else if(dir == Direction.Right)
            return rightSprites[spriteIndex];

        return null;
    }
}
