using UnityEngine;
using System.Collections;

public class Torch : Base
{
    public Sprite lit;
    public Sprite unlit;
    public bool isLit;
    public float timeSinceLastFade;
    public float fadeTime;
    public float fadeTimeBase;
    public float fadeVariation;

    public override void Update()
    {
        base.Update();

        float dt = Time.deltaTime;
        timeSinceLastFade += dt;
        if (timeSinceLastFade >= fadeTime)
        {
            timeSinceLastFade -= fadeTime;
            fadeTime = fadeTimeBase + Random.Range(-fadeVariation, fadeVariation);
            Fade();
        }
    }

    public void OnMouseDown()
    {
        if (isLit) {
            return;
        }

        isLit = true;
        GetComponent<SpriteRenderer>().sprite = lit;
        level.PlayerActed();
    }

    public void Fade()
    {
        isLit = false;
        GetComponent<SpriteRenderer>().sprite = unlit;
    }
}
