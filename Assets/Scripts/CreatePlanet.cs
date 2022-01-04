using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatePlanet : MonoBehaviour
{
    [SerializeField] private GameObject rocket;
    [SerializeField] private GameObject sun;

    private void Awake()
    {
        sun.GetComponent<Planet>().Mass = Planet.sunMass;
        sun.GetComponent<Planet>().Position = Planet.sunPos;
        sun.GetComponent<Planet>().Velocity = Planet.sunVelocity;
    }

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
        Manager.RefreshPlanetArray();
    }
    public void SwitchSun()
    {
        sun.SetActive(!sun.activeInHierarchy);
    }
}
