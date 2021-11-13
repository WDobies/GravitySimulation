using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlanet : ICreate
{
    public override void Create()
    {
        GameObject planet = ObjectPool.Instance.GetPooledObject();
        if (planet)
        {
            planet.SetActive(true);
            planet.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f);
        }        
    }
}
