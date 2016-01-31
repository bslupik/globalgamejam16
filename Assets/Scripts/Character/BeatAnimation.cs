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
            beatRenderer.sprite = sequenceOneSprites[0];
        }
    }
	
	public override void Update() {
		base.Update();

        int nextSpriteIndex = (int) (base.level.timeSinceBeat / timePerSprite());
        if(nextSpriteIndex != spriteIndex)
            loadNextSprite();

        
	}

    private float timePerSprite()
    {
        if(sequenceOneSprites.Length == 0 || sequenceTwoSprites.Length == 0)
        {
            return 0.0f;
        }
        if(sequenceOne)
        {
            return (float) (base.level.timePerBeat / sequenceOneSprites.Length);
        }
        return (float) (base.level.timePerBeat / sequenceTwoSprites.Length);
    }

    private void loadNextSprite()
    {
        if (sequenceOneSprites.Length == 0 || sequenceTwoSprites.Length == 0)
        {
            beatRenderer.sprite = null;
            spriteIndex = -1;
            return;
        }

        if (sequenceOne)
            spriteIndex = (spriteIndex + 1) % sequenceOneSprites.Length;
        else
            spriteIndex = (spriteIndex + 1) % sequenceTwoSprites.Length;

        if (spriteIndex == 0)
            sequenceOne = !sequenceOne;

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
