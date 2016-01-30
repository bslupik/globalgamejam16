using UnityEngine;
using System.Collections;

//basically the same as Invoke() and InvokeRepeating(), but no reflection!

//also includes LINQ-ish functions for doing lerps

//probably will need expansions/reworks when I actually use this in something

/// <summary>
/// Contains static utility functions to encapsulate common <see cref="Coroutine"/> patterns.
/// </summary>
public static class Callback {
    public delegate void CallbackMethod();

    //code that accepts a lerp value from zero to one
    /// <summary>
    /// Delegate functions that can be used in <see cref="DoLerp"/>.
    /// </summary>
    /// <param name="lerpValue">A <see cref="Float"/> between zero and one that represents the current progress.</param>
    public delegate void Lerpable(float lerpValue);

    //keep the autocomplete namespace distinct between the IEnumerators and the coroutines
    /// <summary>
    /// A class to contain all the <see cref="IEnumerator"/> functions so that they don't show up in autocomplete.
    /// </summary>
    public static class Routines
    {
        //basically Invoke
        /// <summary>
        /// Calls the specified code after the specified time has passed.
        /// </summary>
        /// <param name="code">The function to call.</param>
        /// <param name="time">The amount of time to wait before calling the code.</param>
        /// <param name="callingScript">The <see cref="MonoBehaviour"/> to run the Coroutine on.</param>
        /// <param name="mode">The time mode to run the Coroutine with.</param>
        /// <returns></returns>
        public static IEnumerator FireAndForgetRoutine(CallbackMethod code, float time, MonoBehaviour callingScript, Mode mode = Mode.UPDATE)
        {
            switch(mode)
            {
                case Mode.UPDATE:
                case Mode.FIXEDUPDATE:
                    yield return new WaitForSeconds(time);
                    break;
                case Mode.REALTIME:
                    yield return callingScript.StartCoroutine(WaitForRealSecondsRoutine(time));
                    break;
            }
            code();
        }

        //Fires the code on the next update/fixed update. Lazy way to keep the code from affecting what you're doing right now
        /// <summary>
        /// Calls the specified code on the next Update().
        /// </summary>
        /// <param name="code">The function to call.</param>
        /// <param name="mode">The time mode to run the Coroutine with.</param>
        /// <returns></returns>
        public static IEnumerator FireForUpdateRoutine(CallbackMethod code, Mode mode = Mode.UPDATE)
        {
            switch(mode)
            {
                case Mode.UPDATE:
                case Mode.REALTIME:
                    yield return null;
                    break;
                case Mode.FIXEDUPDATE:
                    yield return new WaitForFixedUpdate();
                    break;
            }
            code();
        }

        // TODO : replace with a YieldInstruction override

        //same as WaitForSeconds(), but is not affected by timewarping
        /// <summary>
        /// Waits for the specified real time.
        /// </summary>
        /// <param name="seconds">The amount of time to wait.</param>
        /// <returns></returns>
        public static IEnumerator WaitForRealSecondsRoutine(float seconds)
        {
            float pauseStartTime = Time.realtimeSinceStartup;
            float pauseEndTime = pauseStartTime + seconds;

            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
        }

        //does a standard coroutine Lerp on a bit of code, from zero to one by default.
        /// <summary>
        /// Calls the code with the current progress over the specified time.
        /// </summary>
        /// <param name="code">The lerpable code to call.</param>
        /// <param name="time">The duration of the lerp.</param>
        /// <param name="callingScript">The <see cref="MonoBehaviour"/> to run the Coroutine on.</param>
        /// <param name="reverse">If <c>True</c>, calls the lerpable from 0 to 1 instead of 1 to zero.</param>
        /// <param name="mode">The time mode to run the Coroutine with.</param>
        /// <returns></returns>
        public static IEnumerator DoLerpRoutine(Lerpable code, float time, MonoBehaviour callingScript, bool reverse = false, Mode mode = Mode.UPDATE)
        {
            IEnumerator routine = null;
            switch (mode)
            {
                case Mode.UPDATE:
                    routine = DoLerpUpdateTimeRoutine(code, time, reverse);
                    break;
                case Mode.FIXEDUPDATE:
                    routine = DoLerpFixedTimeRoutine(code, time, reverse);
                    break;
                case Mode.REALTIME:
                    routine = DoLerpRealtimeRoutine(code, time, reverse);
                    break;
            }
            yield return callingScript.StartCoroutine(routine);
            code(reverse?0:1);
        }

        public static IEnumerator DoLerpUpdateTimeRoutine(Lerpable code, float time, bool reverse = false)
        {
            if (!reverse)
            {
                float timeElapsed = 0;
                while (timeElapsed < time)
                {
                    code(timeElapsed / time);
                    yield return null;
                    timeElapsed += Time.deltaTime;
                }
            }
            else
            {
                float timeRemaining = time;
                while (timeRemaining > 0)
                {
                    code(timeRemaining / time);
                    yield return null;
                    timeRemaining -= Time.deltaTime;
                }
            }
        }

        //same, but run the lerp code independent of any timewarping
        public static IEnumerator DoLerpRealtimeRoutine(Lerpable code, float time, bool reverse = false)
        {
            float realStartTime = Time.realtimeSinceStartup;
            float realEndTime = realStartTime + time;
            if (!reverse)
            {
                while (Time.realtimeSinceStartup < realEndTime)
                {
                    code((Time.realtimeSinceStartup - realStartTime) / time);
                    yield return null;
                }
            }
            else
            {
                while (Time.realtimeSinceStartup < realEndTime)
                {
                    code((realEndTime - Time.realtimeSinceStartup) / time);
                    yield return null;
                }
            }
        }

        //used Time.FixedDeltaTime instead of delta time (for important physics/gameplay things)
        public static IEnumerator DoLerpFixedTimeRoutine(Lerpable code, float time, bool reverse = false)
        {
            if (!reverse)
            {
                float timeElapsed = 0;
                while (timeElapsed < time)
                {
                    code(timeElapsed / time);
                    yield return new WaitForFixedUpdate();
                    timeElapsed += Time.fixedDeltaTime;
                }
            }
            else
            {
                float timeRemaining = time;
                while (timeRemaining > 0)
                {
                    code(timeRemaining / time);
                    yield return new WaitForFixedUpdate();
                    timeRemaining -= Time.fixedDeltaTime;
                }
            }

            code(reverse ? 0 : 1);
        }
        /// <summary>
        /// Executes the code after the Coroutine has finished.
        /// </summary>
        /// <param name="waitFor">The Coroutine to wait for.</param>
        /// <param name="code">The Code to execute after the Coroutine has finished</param>
        /// <returns></returns>
        public static IEnumerator WaitForRoutine(Coroutine waitFor, CallbackMethod code)
        {
            yield return waitFor;
            code();
        }
    }

    private static Coroutine RunIfActiveAndEnabled(MonoBehaviour callingScript, IEnumerator code)
    {
        if (callingScript.isActiveAndEnabled)
            return callingScript.StartCoroutine(code);
        else
            return null;
    }

    //wrappers for the routines in the Routines class so that we don't need to call StartCoroutine()
    /// <summary>
    /// Calls the specified code after the specified time has passed.
    /// </summary>
    /// <param name="code">The function to call.</param>
    /// <param name="time">The amount of time to wait before calling the code.</param>
    /// <param name="callingScript">The <see cref="MonoBehaviour"/> to run the Coroutine on.</param>
    /// <param name="mode">The time mode to run the Coroutine with.</param>
    /// <returns></returns>
    public static Coroutine FireAndForget(this CallbackMethod code, float time, MonoBehaviour callingScript, Mode mode = Mode.UPDATE)
    {
        return RunIfActiveAndEnabled(callingScript, Routines.FireAndForgetRoutine(code, time, callingScript, mode));
    }

    //Fires the code on the next update/fixed update. Lazy way to keep the code from affecting what you're doing right now
    /// <summary>
    /// Calls the specified code on the next Update().
    /// </summary>
    /// <param name="code">The function to call.</param>
    /// <param name="callingScript">The <see cref="MonoBehaviour"/> to run the Coroutine on.</param>
    /// <param name="mode">The time mode to run the Coroutine with.</param>
    /// <returns></returns>
    public static Coroutine FireForUpdate(this CallbackMethod code, MonoBehaviour callingScript, Mode mode = Mode.UPDATE)
    {
        return RunIfActiveAndEnabled(callingScript, Routines.FireForUpdateRoutine(code, mode));
    }

    /// <summary>
    /// Waits for the specified real time.
    /// </summary>
    /// <param name="seconds">The amount of time to wait.</param>
    /// <param name="callingScript">The <see cref="MonoBehaviour"/> to run the Coroutine on.</param>
    /// <returns></returns>
    public static Coroutine WaitForRealSeconds(float seconds, MonoBehaviour callingScript)
    {
        return RunIfActiveAndEnabled(callingScript, Routines.WaitForRealSecondsRoutine(seconds));
    }

    /// <summary>
    /// Calls the code with the current progress over the specified time.
    /// </summary>
    /// <param name="code">The lerpable code to call.</param>
    /// <param name="time">The duration of the lerp.</param>
    /// <param name="callingScript">The <see cref="MonoBehaviour"/> to run the Coroutine on.</param>
    /// <param name="reverse">If <c>True</c>, calls the lerpable from 0 to 1 instead of 1 to zero.</param>
    /// <param name="mode">The time mode to run the Coroutine with.</param>
    /// <returns></returns>
    public static Coroutine DoLerp(Lerpable code, float time, MonoBehaviour callingScript, bool reverse = false, Mode mode = Mode.UPDATE)
    {
        return RunIfActiveAndEnabled(callingScript, Routines.DoLerpRoutine(code, time, callingScript, reverse, mode));
    }

    /// <summary>
    /// Executes the code after the Coroutine has finished.
    /// </summary>
    /// <param name="toFollow">The Coroutine to wait for.</param>
    /// <param name="code">The Code to execute after the Coroutine has finished.</param>
    /// <param name="callingScript">The <see cref="MonoBehaviour"/> to run the Coroutine on.</param>
    /// <returns></returns>
    public static Coroutine FollowedBy(this Coroutine toFollow, CallbackMethod code, MonoBehaviour callingScript)
    {
        return RunIfActiveAndEnabled(callingScript, Routines.WaitForRoutine(toFollow, code));
    }
    /// <summary>
    /// Defines which time mode to run Coroutines with.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Steps the Coroutines every Update(). Is affected by Time.timeScale.
        /// </summary>
        UPDATE,
        /// <summary>
        /// Steps the Coroutines every FixedUpdate(). Is affected by Time.timeScale. Useful for callbacks that will be affecting physics or other time-crucial code.
        /// </summary>
        FIXEDUPDATE,
        /// <summary>
        /// Steps the Coroutines every frame. Is NOT affected by Time.timeScale.
        /// </summary>
        REALTIME
    }
}