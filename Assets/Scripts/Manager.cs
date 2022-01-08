using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Planet[] planets;

    public static float GravityConstant = 4.67f /** (float)Math.Pow(10, -11)*/;
    public static float TimeStep = 0.01f;

    public static Manager Instance 
    {
        get => _instance;
    }

    public static void RefreshPlanetArray()
    {
        planets = FindObjectsOfType<Planet>();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (!planets[i].gameObject.activeInHierarchy) continue;
            planets[i].UpdateVelocity(planets, TimeStep);
        }

        for (int i = 0; i < planets.Length; i++)
        {
            if (!planets[i].gameObject.activeInHierarchy) continue;
            planets[i].UpdatePosition(TimeStep);
        }
    }

    private void Awake()
    {
        Time.fixedDeltaTime = TimeStep;
        _instance = this;
        RefreshPlanetArray();
    }
}
