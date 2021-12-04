using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    //TO FIX
    #region Range
    static public float minMass = 1f;
    static public float maxMass = 2000f;

    static public float minPos = -30f; // x,y,z
    static public float maxPos = 30f;

    static public float minVelocity = 0f;
    static public float maxVelocity = 10f;

    static public float minVelocityDirction = 0f; // x,y,z
    static public float maxVelocityDirction = 360f;

    #endregion

    private Vector3 _position;
    private float _currentRadius;
    private float _mass;
    public Rigidbody rb;

    public float Mass 
    { get => _mass; 

      set 
        {
            rb.mass = _mass = value;
        } 
    } // 10^24 kg (1 - 2000) range 

    public Vector3 velocity { get; set; }
    public Vector3 initialVelocity;

    public float Velocity { get; set; } // km/s
    public Vector3 VelocityDirection { get; set; } //rotation
    public Vector3 Position
    {
        get => _position;

        set
        {
            rb.position = _position = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = _mass;
        velocity = initialVelocity;
        GenerateRandomProperties();
    }

    private void Update()
    {
        float radius = Mathf.InverseLerp(1, 2000, Mass) * 10;
        if (radius != _currentRadius)
        {
            _currentRadius = radius;
            transform.localScale = new Vector3(_currentRadius, _currentRadius, _currentRadius);
        }
    }

    private void GenerateRandomProperties()
    {
        Mass = Random.Range(minMass, maxMass);
        Position = new Vector3(Random.Range(minPos, maxPos), Random.Range(minPos, maxPos), Random.Range(minPos, maxPos));
        Velocity = 1; //to fix
        VelocityDirection = Vector3.one; //to fix
    }

    public void UpdateVelocity(Planet[] allPlanets, float timeStep)
    {
        foreach (var planet in allPlanets)
        {
            if(planet != this)
            {
                float sqrDst = (planet.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (planet.rb.position - rb.position).normalized; 
                Vector3 acceleration = forceDir * Manager.GravityConstant * planet._mass / sqrDst;  //gravity Const
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdatePosition(float timeStep)
    {
        Position += velocity * timeStep;

    }
}