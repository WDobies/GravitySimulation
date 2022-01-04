using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Planet
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = _mass;
        GenerateRandomProperties();
        Manager.RefreshPlanetArray();
    }
    private void Update()
    {
        // Rotate the rocket depending on its velocity
        transform.rotation = Quaternion.LookRotation(Velocity);
    }
}
