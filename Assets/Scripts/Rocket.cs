using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Planet
{
    private void Update()
    {
        // Rotate the rocket depending on its velocity
        transform.rotation = Quaternion.LookRotation(Velocity);
    }
}
