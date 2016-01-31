using UnityEngine;
using System.Collections;

public class Ghost : TimedLife
{
    public float health;
    public float maxHealth = 100.0f;
    public float clickDamage = 100.0f;

    [SerializeField]
    protected Material deathMat;

	public override void Start()
	{
		base.Start();
        health = maxHealth;
    }
	
	public override void Update()
	{
		base.Update();
        if (health <= 0.0f)
        {
            OnDeath();
        }
	}

    public void OnMouseDown()
    {
        level.PlayerActed();
        health -= clickDamage;
    }

    public override void OnDeath()
    {
        if (health > 0)
        {
            level.GhostEscaped();
            StartCoroutine(FadeOut());
        }
        else
        {
            level.GhostClicked();
            StartCoroutine(Death());
        }
    }

    public void Fade(float fadeTime)
    {
        Color currentColor = GetComponent<SpriteRenderer>().color;
        currentColor.a = fadeTime;
        GetComponent<SpriteRenderer>().color = currentColor;
    }

    IEnumerator FadeOut()
    {
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        yield return Callback.DoLerp((float l) => Fade(l), 1.0f, this, reverse: true);
        Destroy(this.gameObject);
    }

    IEnumerator Death()
    {
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.material = Instantiate(deathMat);
        yield return Callback.DoLerp((float l) => rend.material.SetFloat(Tags.ShaderParams.cutoff, l), 0.5f, this, reverse: true);
        Destroy(this.gameObject);
    }
}
