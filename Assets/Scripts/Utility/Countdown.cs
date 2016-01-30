using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapper class for IEnumerator Coroutines that simplifies pausing and restarting
/// </summary>
public class Countdown {

    //delegate types
    public delegate IEnumerator GetIEnumerator();

    GetIEnumerator source;
    IEnumerator countdown;
    IEnumerator countdownWrapper;
    MonoBehaviour script;

    public bool active
    {
        get { return countdown != null && !paused; }
    }

    bool paused = false;
    public bool Paused { get { return paused; } }

    public Countdown(GetIEnumerator source, MonoBehaviour script)
    {
        this.source = source;
        this.script = script;
    }

    public Countdown(GetIEnumerator source, MonoBehaviour script, bool playOnAwake) : this(source, script)
    {
        if (playOnAwake)
            Play();
    }

    public bool Play()
    {
        if (paused)
        {
            script.StartCoroutine(countdown);
            script.StartCoroutine(countdownWrapper);
            paused = false;
            return true;
        }
        else if (countdown == null)
        {
            countdownWrapper = CountdownWrapper();
            script.StartCoroutine(countdownWrapper);
            return true;
        }
        return false;
    }

    public void Restart()
    {
        if (countdown != null)
        {
            paused = false; //the if to check if it's true is redundant
            script.StopCoroutine(countdown);
            script.StopCoroutine(countdownWrapper);
        }
        countdownWrapper = CountdownWrapper();
        script.StartCoroutine(countdown);
    }

    public bool Pause()
    {
        if (active)
        {
            script.StopCoroutine(countdown);
            script.StopCoroutine(countdownWrapper);
            paused = true;
            return true;
        }
        return false;
    }

    public bool Stop()
    {
        if (countdown != null)
        {
            paused = false; //the if to check if it's true is redundant
            script.StopCoroutine(countdown);
            script.StopCoroutine(countdownWrapper);
            countdown = null;
            countdownWrapper = null;
            return true;
        }
        return false;
    }

    public IEnumerator CountdownWrapper()
    {
        countdown = source();
        yield return script.StartCoroutine(countdown);
        countdown = null;
    }
}
