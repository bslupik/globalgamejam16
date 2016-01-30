using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PunctureLevel : Level {

    [SerializeField]
    protected int[] puncturePointNumbers;

    Queue<int> sortedPointNumbers;

	// Use this for initialization
	void Awake () {
        Array.Sort(puncturePointNumbers);
        sortedPointNumbers = new Queue<int>(puncturePointNumbers);
	}

    public bool PlayerActed(int order)
    {
        if (order == sortedPointNumbers.Peek())
        {
            sortedPointNumbers.Dequeue();
            PlayerActed();
            return true;
        }
        else
        {
            return false;
        }
    }
}
