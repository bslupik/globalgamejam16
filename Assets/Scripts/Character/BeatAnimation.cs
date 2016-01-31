using UnityEngine;
using System.Collections;

public class BeatAnimation : Base {

    public SpriteRenderer beatRenderer;
    public Sprite[] sequenceOneSprites;
    public Sprite[] sequenceTwoSprites;
    bool sequenceOne = true;
    int spriteIndex;

    public override void Start () {
		base.Start();
        if (sequenceOneSprites.Length > 0)
        {
            spriteIndex = 0;
            if(beatRenderer != null)
                beatRenderer.sprite = sequenceOneSprites[0];
        }
    }
	
	public override void Update() {
		base.Update();

        int nextSpriteIndex = (int) (level.timeSinceBeat / timePerSprite());
        if(nextSpriteIndex != spriteIndex)
            loadNextSprite();
	}

    public Sprite GetSprite()
    {
        if(spriteIndex == -1)
            return null;

        if(sequenceOne)
        {
            return sequenceOneSprites[spriteIndex];
        }
        return sequenceTwoSprites[spriteIndex];
    }

    private float timePerSprite()
    {
        if(sequenceOneSprites.Length == 0)
        {
            return 0.0f;
        }
        if(sequenceOne)
        {
            return (float) (level.timePerBeat / sequenceOneSprites.Length);
        }
        return (float) (level.timePerBeat / sequenceTwoSprites.Length);
    }

    private void loadNextSprite()
    {
        if (sequenceOneSprites.Length == 0 || sequenceTwoSprites.Length == 0)
        {
            spriteIndex = -1;
            return;
        }

        if (sequenceOne)
            spriteIndex = (spriteIndex + 1) % sequenceOneSprites.Length;
        else
            spriteIndex = (spriteIndex + 1) % sequenceTwoSprites.Length;

        if (spriteIndex == 0 && sequenceTwoSprites.Length > 0)
            sequenceOne = !sequenceOne;

        if(beatRenderer != null)
        {
            if (sequenceOne)
            {
                beatRenderer.sprite = sequenceOneSprites[spriteIndex];
            }
            else
            {
                beatRenderer.sprite = sequenceTwoSprites[spriteIndex];
            }
        }
    }
}
