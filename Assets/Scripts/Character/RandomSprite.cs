using UnityEngine;
using System.Collections;

public class RandomSprite : Base
{
    public Sprite[] sprites;

	public override void Start()
	{
		base.Start();
        if (sprites != null && sprites.Length > 0)
        {
        }
	}
}
