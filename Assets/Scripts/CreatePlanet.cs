using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlanet : MonoBehaviour
{
    [SerializeField] private GameObject rocket;
    public void Create()
    {
        GameObject planet = ObjectPool.Instance.GetPooledObject();
        if (planet)
        {
            planet.SetActive(true);
            planet.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f);
        }        
    }
    public void SwitchRocket()
    {
        rocket.SetActive(!rocket.activeInHierarchy);
    }
}
