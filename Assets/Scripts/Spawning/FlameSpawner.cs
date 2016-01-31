using UnityEngine;
using System.Collections;

public class FlameSpawner : Spawner
{
    public float flameLifeBase;
    public float flameLifeVariation;

    public override GameObject Spawn()
    {
        GameObject instantiatedObject = base.Spawn();
        instantiatedObject.GetComponent<TimedLife>().lifeTime = 0;
        instantiatedObject.GetComponent<TimedLife>().lifeTimeBase = flameLifeBase;
        instantiatedObject.GetComponent<TimedLife>().lifeTimeVariation = flameLifeVariation;
        return instantiatedObject;
    }
}
